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
        struct UploadData
        {
            public IElectionProvider Provider;
            public UserEntity User;
            public CommissionEntity Commission;
            public ElectionEntity Election;
            public CandidateEntity[] Candidates;
        }

        WorkerHandle Upload;

        public event Action UploadCompleted;
        public event Action<string> UploadRejected;

        void IVotingService.BeginUpload(ElectionEntity election)
        {
            IElectionProvider provider = this._provider;
            UserEntity user = ServiceManager.Instance.CurrentUser;
            CommissionEntity commission = App.CurrentAppData.CurrentCommision;
            CandidateEntity[] candidates = App.CurrentAppData.Candidates;

            WorkerContext context = new WorkerContext()
            {
                DoWork = uploadProcess,
                RunWorkerCompleted = uploadCompleted,
                UserData = new UploadData()
                {
                    Provider = provider,
                    User = user,
                    Commission = commission,
                    Election = election,
                    Candidates = candidates
                }
            };

            Upload = Begin(context);

            // tutaj cały czas trwa procedura logowania
            // ...
        }

        /// <summary>
        /// Procedura wysyłania wyników wyborów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void uploadProcess(object sender, DoWorkEventArgs e)
        {
            var provider = ((UploadData)e.Argument).Provider;
            var user = ((UploadData)e.Argument).User;
            var commission = ((UploadData)e.Argument).Commission;
            var election = ((UploadData)e.Argument).Election;
            var candidates = ((UploadData)e.Argument).Candidates;

            bool result = provider.UploadResults(user, commission, election, candidates);
            if (result == false)
            {
                throw new UploadException(
                    UploadException.ErrorReason.CannotSendResults);
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

        /// <summary>
        /// Rodzaj wyjątku używany w procesie przesyłania danych wyborów.
        /// </summary>
        public class UploadException : ApplicationException
        {
            public enum ErrorReason
            {
                CannotSendResults,
            }

            private ErrorReason _reason;
            private Dictionary<ErrorReason, string> _messages;

            public UploadException(ErrorReason reason)
            {
                this._reason = reason;
                this._messages = new Dictionary<ErrorReason, string>()
                {
                    { ErrorReason.CannotSendResults, "Teraz nie można wysłać danych o wynikach wyborów. Poczekaj chwilę i spróbuj ponownie." },
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
