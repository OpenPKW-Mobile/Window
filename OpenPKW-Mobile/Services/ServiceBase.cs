using OpenPKW_Mobile.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    abstract class ServiceBase
    {
        protected class WorkerHandle
        {
            private Guid _guid = Guid.NewGuid();
            public override string ToString()
            {
                return _guid.ToString();
            }
        }

        protected class WorkerContext
        {
            public DoWorkEventHandler DoWork;
            public ProgressChangedEventHandler ProgressChanged;
            public RunWorkerCompletedEventHandler RunWorkerCompleted;
            public object UserData;            
        }

        class Worker : BackgroundWorkerEx<WorkerContext> 
        {
            public readonly WorkerHandle Handle;

            public Worker(WorkerContext context) 
                : base(context)
            {
                this.Handle = new WorkerHandle();
            
                if (context.DoWork != null)
                    this.DoWork += context.DoWork;
                if (context.ProgressChanged != null)
                    this.ProgressChanged += context.ProgressChanged;
                if (context.RunWorkerCompleted != null)
                    this.RunWorkerCompleted += context.RunWorkerCompleted;
            }
        }

        private object _locker;
        private Dictionary<WorkerHandle, Worker> _workers;

        public ServiceBase()
        {
            this._locker = new Object();
            this._workers = new Dictionary<WorkerHandle, Worker>();
        }

        protected WorkerHandle Begin(WorkerContext context)
        {
            var workers = this._workers;

            Worker worker = new Worker(context);
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;
            worker.RunWorkerAsync(context.UserData);
            worker.WorkerSupportsCancellation = true;
            worker.WorkerReportsProgress = (context.ProgressChanged != null);

            lock(_locker)
            {
                workers.Add(worker.Handle, worker);
            }
   
            // tutaj cały czas pracuje w tle wątek
            //....

            return worker.Handle;
        }

        protected void Cancel(WorkerHandle handle)
        {
            var workers = this._workers;

            lock(_locker)
            {
                if (workers.ContainsKey(handle))
                {
                    Worker worker = workers[handle];
                    worker.CancelAsync();
                }
            }
        }

        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var workers = this._workers;
            var worker = (Worker)sender;

            lock (_locker)
            {
                if (workers.ContainsKey(worker.Handle))
                {
                    workers.Remove(worker.Handle);
                }
            }
        }
    }
}
