using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Services
{
    partial class VotingService
    {
        /// <summary>
        /// Zestaw danych przekazywanych pomiędzy wątkami.
        /// </summary>
        struct FetchData
        {
            public IElectionProvider Provider;
            public UserEntity User;
            public CommissionEntity Commission;
        }

        WorkerHandle Fetch;

        public event Action<CandidateEntity[]> FetchCompleted;
        public event Action<string> FetchRejected;

        void IVotingService.BeginFetch()
        {
            IElectionProvider provider = this._provider;
            UserEntity user = ServiceManager.Instance.CurrentUser;
            CommissionEntity commission = App.CurrentAppData.CurrentCommision;

            WorkerContext context = new WorkerContext()
            {
                DoWork = fetchProcess,
                RunWorkerCompleted = fetchCompleted,
                UserData = new FetchData()
                {
                    Provider = provider,
                    User = user,
                    Commission = commission
                }
            };

            Fetch = Begin(context);

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
            var provider = ((FetchData)e.Argument).Provider;
            var user = ((FetchData)e.Argument).User;
            var commission = ((FetchData)e.Argument).Commission;

            CandidateEntity[] candidates = provider.GetCandidates(user, commission);
            if (candidates == null)
            {
                throw new FetchException(
                    FetchException.ErrorReason.CannotLoadCandidates);
            }
            else if (candidates.Length == 0)
            {
                throw new FetchException(
                    FetchException.ErrorReason.NobodyCandidate);
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
        /// Rodzaj wyjątku używany w procesie przesyłania danych wyborów.
        /// </summary>
        public class FetchException : ApplicationException
        {
            public enum ErrorReason
            {
                NobodyCandidate,
                CannotLoadCandidates,
            }

            private ErrorReason _reason;
            private Dictionary<ErrorReason, string> _messages;

            public FetchException(ErrorReason reason)
            {
                this._reason = reason;
                this._messages = new Dictionary<ErrorReason, string>()
                {
                    { ErrorReason.NobodyCandidate, "System nie posiada wiedzy o kandydatach w tych wyborach." },
                    { ErrorReason.CannotLoadCandidates, "Nie udało się uzyskać informacji o liście osób kandydujących w wyborach." },
                };
            }

            public override string Message
            {
                get
                {
                    return this._messages[_reason];
                }
            }
        }


    }
}
