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
    }
}
