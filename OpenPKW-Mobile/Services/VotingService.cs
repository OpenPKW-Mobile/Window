using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Providers;
using OpenPKW_Mobile.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    partial class VotingService : ServiceBase, IVotingService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct WorkerData
        {
            public IElectionProvider ElectionProvider;
            public ElectionEntity Election;
        }

        WorkerHandle Fetch;
        WorkerHandle Upload;

        public event Action<CandidateEntity[]> FetchCompleted;
        public event Action<string> FetchRejected;
        public event Action UploadCompleted;
        public event Action<string> UploadRejected;    

        /// <summary>
        /// Dostawca usługi głosowania.
        /// </summary>
        private IElectionProvider _provider;

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="provider"></param>
        public VotingService(IElectionProvider provider)
        {
            this._provider = provider;
        }

        void IVotingService.BeginFetch()
        {
            IElectionProvider provider = this._provider;
            WorkerContext context = new WorkerContext()
            {
                DoWork = fetchProcess,
                RunWorkerCompleted = fetchCompleted,
                UserData = new WorkerData()
                {
                    ElectionProvider = provider
                }
            };

            Fetch = Begin(context);

            // tutaj cały czas trwa procedura logowania
            // ...
        }

        void IVotingService.BeginUpload(ElectionEntity election)
        {
            IElectionProvider provider = this._provider;
            WorkerContext context = new WorkerContext()
            {
                DoWork = uploadProcess,
                RunWorkerCompleted = uploadCompleted,
                UserData = new WorkerData()
                {
                    ElectionProvider = provider,
                    Election = election
                }
            };

            Upload = Begin(context);

            // tutaj cały czas trwa procedura logowania
            // ...
        }

        /// <summary>
        /// Procedura pobierania listy kandydatów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fetchProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((WorkerData)e.Argument).ElectionProvider;

            CandidateEntity[] candidates = provider.GetCandidates();
            if (candidates == null)
            {
                throw new VotingException(
                    VotingException.ErrorReason.CannotLoadCandidates);
            }

            e.Result = candidates;
        }

        private void fetchCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                if (FetchRejected != null)
                    FetchRejected(e.Error.Message);
            }
            else
            {
                CandidateEntity[] candidates = (CandidateEntity[])e.Result;
                if (FetchCompleted != null)
                    FetchCompleted(candidates);
            }
        }

        /// <summary>
        /// Procedura wysyłania wyników wyborów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((WorkerData)e.Argument).ElectionProvider;
            var election = ((WorkerData)e.Argument).Election;

            bool result = provider.SendResults(election);
            if (result == false)
            {
                throw new VotingException(
                    VotingException.ErrorReason.CannotSendResults);
            }

            e.Result = result;
        }

        private void uploadCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                if (UploadRejected != null)
                    UploadRejected(e.Error.Message);
            }
            else
            {
                if (UploadCompleted != null)
                    UploadCompleted();
            }
        }


    }
}
