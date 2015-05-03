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
    class VotingService : IVotingService
    {
        /// <summary>
        /// Rodzaj wykonywanej operacji.
        /// </summary>
        enum WorkType { FetchCandidates, SendResults }
   
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct WorkerData
        {
            public IElectionProvider ElectionProvider;
            public ElectionEntity Election; 
        }
       
        /// <summary>
        /// Dostawca usługi głosowania.
        /// </summary>
        private IElectionProvider _provider;

        /// <summary>
        /// Zarządza zadaniami w tle.
        /// </summary>
        private BackgroundWorkerEx<WorkType> _worker;

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        /// <param name="provider"></param>
        public VotingService(IElectionProvider provider)
        {
            this._provider = provider;
        }

        /// <summary>
        /// Obsługa wyniku procedury logowania.
        /// </summary>
        /// <param name="sender">Nadawca zdarzenia.</param>
        /// <param name="e">Wyniki przetwarzania.</param>
        void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            var worker = (BackgroundWorkerEx<WorkType>)sender;
            if (e.Error != null)
            {
                // poinformowanie słuchaczy o problemie
                switch(worker.Type)
                {
                    case WorkType.FetchCandidates:
                        if (FetchRejected != null)
                            FetchRejected(e.Error.Message);
                        break;
                    case WorkType.SendResults:
                        if (SendRejected != null)
                            SendRejected(e.Error.Message);
                        break;
                }
            }
            else
            {
                // poinformowanie słuchaczy o poprawnym wyniku logowania
                // od tego momentu użytkownik jest identyfikowany poprzez obiekt typu [UserEntity]
                switch (worker.Type)
                {
                    case WorkType.FetchCandidates:
                        CandidateEntity[] candidates = (CandidateEntity[])e.Result;
                        if (FetchCompleted != null)
                            FetchCompleted(candidates);
                        break;
                    case WorkType.SendResults:
                        if (SendCompleted != null)
                            SendCompleted();
                        break;
                }
            }

            this._worker = null;
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

        /// <summary>
        /// Procedura wysyłania wyników wyborów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sendProcess(object sender, DoWorkEventArgs e)
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

        #region Implementacja IVotingService
        public event Action<CandidateEntity[]> FetchCompleted;
        public event Action<string> FetchRejected;
        public event Action SendCompleted;
        public event Action<string> SendRejected;

        void IVotingService.BeginFetchCandidates()
        {
            IElectionProvider provider = this._provider;

            if (this._worker != null)
                throw new Exception();

            // przygotowanie usługi do pracy w tle
            var worker = new BackgroundWorkerEx<WorkType>(WorkType.FetchCandidates);
            worker.DoWork += fetchProcess;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            WorkerData data = new WorkerData()
            {
                ElectionProvider = provider
            };

            // uruchomienie procedury logowania w osobnym wątku
            worker.RunWorkerAsync(data);

            this._worker = worker;

            // tutaj cały czas trwa procedura logowania
            // ...
        }

        void IVotingService.BeginSendResults(ElectionEntity election)
        {
            IElectionProvider provider = this._provider;

            if (this._worker != null)
                throw new Exception();

            // przygotowanie usługi do pracy w tle
            var worker = new BackgroundWorkerEx<WorkType>(WorkType.SendResults);
            worker.DoWork += sendProcess;
            worker.RunWorkerCompleted += worker_RunWorkerCompleted;

            WorkerData data = new WorkerData()
            {
                ElectionProvider = provider,
                Election = election
            };

            // uruchomienie procedury logowania w osobnym wątku
            worker.RunWorkerAsync(data);

            this._worker = worker;

            // tutaj cały czas trwa procedura logowania
            // ...
        }
        #endregion

    }
}
