using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile
{
    /// <summary>
    /// Menadżer usług udostępnionych przez zewnętrzne aplikacje.
    /// </summary>
    class ServiceManager
    {
        #region Singleton
        private static ServiceManager _instance = null;
        public static ServiceManager Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServiceManager();
                return _instance;
            }
        }

        private ServiceManager()
        {

        }
        #endregion

        /// <summary>
        /// Udostępnieni serwisu logowania.
        /// </summary>
        private ILoginService _login = null;
        public ILoginService Login
        {
            get
            {
                if (_login == null)
                    _login = ServiceFactory.Login;
                return _login;
            }
        }

        /// <summary>
        /// Bieżący użytkownik aplikacji.
        /// </summary>
        public UserEntity CurrentUser { get; set; }
    }
}
