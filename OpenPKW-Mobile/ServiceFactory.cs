using OpenPKW_Mobile.Providers;
using OpenPKW_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile
{
    static class ServiceFactory
    {
        /// <summary>
        /// Dostawca usługi logowania.
        /// </summary>
        public static IAuthenticationProvider AuthenticationProvider;
        
        /// <summary>
        /// Dostawca usługi głosowania.
        /// </summary>
        public static IElectionProvider ElectionProvider;

        /// <summary>
        /// Dostawca usługi głosowania.
        /// </summary>
        public static IStorageProvider StorageProvider;

        /// <summary>
        /// Tworzy nową usługę logowania.
        /// </summary>
        public static LoginService Login
        {
            get
            {
                return new LoginService(AuthenticationProvider);
            }
        }

        /// <summary>
        /// Tworzy nową usługę głosowania.
        /// </summary>
        public static VotingService Voting
        {
            get
            {
                return new VotingService(ElectionProvider);
            }
        }

        /// <summary>
        /// Tworzy nową usługę głosowania.
        /// </summary>
        public static PhotoService Photo
        {
            get
            {
                return new PhotoService(StorageProvider);
            }
        }

    }
}
