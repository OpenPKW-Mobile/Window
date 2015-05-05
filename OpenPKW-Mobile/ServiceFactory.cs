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
        /// Tworzy nową usługę odzyskiwaia hasła.
        /// </summary>
        public static RemindService Remind
        {
            get
            {
                return new RemindService(AuthenticationProvider);
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

    }
}
