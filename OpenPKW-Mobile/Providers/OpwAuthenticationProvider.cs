using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    class OpwAuthenticationProvider : ProviderBase, IAuthenticationProvider
    {
        const string URI_LOGIN = "http://dev.otwartapw.pl/opw/service/user/login";
        const string URI_LOGOUT = "http://dev.otwartapw.pl/opw/service/user/logout";
        const string URI_COMMISSIONS = "http://dev.otwartapw.pl/opw/service/user/{0}/obwodowa";
        const string URI_REMIND = "http://dev.otwartapw.pl/opw/service/user/remind";
        const string URI_REGISTER = "http://dev.otwartapw.pl/opw/service/user/register";

        const string API_CLIENT = "OpenPKW-WindowsPhone";
        const string API_TOKEN = "d171794c5c1f7a50aeb8f7056ab84a4fbcd6fbd594b1999bddaefdd03efc0591";
        
        #region Komunikaty

        [DataContract]
        struct LoginResponseData
        {
            [DataMember(Name = "id")]
            public int UserID { get; set; }

            [DataMember(Name = "fullname")]
            public string FullName { get; set; }

            [DataMember(Name = "firstname")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastname")]
            public string LastName { get; set; }

            [DataMember(Name = "login")]
            public string UserLogin { get; set; }

            [DataMember(Name = "token")]
            public string SessionToken { get; set; }

            [DataMember(Name = "sessionActive")]
            public bool IsSessionActive { get; set; }

            [DataMember(Name = "sessionTimeout")]
            public string SessionTimeout { get; set; }
        }

        [DataContract]
        struct RemindRequestData
        {
            [DataMember(Name = "name")]
            public string UserName { get; set; }

            [DataMember(Name = "email")]
            public string Email { get; set; }
        }

        [DataContract]
        struct RegisterRequestData
        {
            [DataMember(Name = "firstname")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastname")]
            public string LastName { get; set; }

            [DataMember(Name = "email")]
            public string Email { get; set; }

            [DataMember(Name = "phone")]
            public string Phone { get; set; }
        }

        #endregion

        #region Implementacja IAuthenticationProvider

        UserEntity IAuthenticationProvider.UserLogin(string userName, string userPassword)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = userName;
                headers["X-OPW-password"] = userPassword;
                var loginUri = new Uri(URI_LOGIN);
                var loginTask = GetResponse(loginUri, headers);
                loginTask.Wait();
                var loginResponse = JsonHelper.FromJson<LoginResponseData>(loginTask.Result);

                return new UserEntity()
                {
                    UserID = loginResponse.UserID.ToString(),
                    LoginName = loginResponse.UserLogin,
                    FirstName = loginResponse.FirstName,
                    LastName = loginResponse.LastName,
                    AuthenticationToken = loginResponse.SessionToken,
                };
            }
            catch
            {
                return null;
            }
        }

        bool IAuthenticationProvider.UserLogout(UserEntity user)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = user.LoginName;
                headers["X-OPW-token"] = user.AuthenticationToken;
                var logoutUri = new Uri(URI_LOGOUT);
                var logoutTask = GetResponse(logoutUri, headers);
                logoutTask.Wait();

                return true;
            }
            catch
            {
                return false;
            }
        }

        bool IAuthenticationProvider.IsSessionValid(UserEntity user)
        {
            try
            {
                // brakuje w API odpowiedniej funkcji
                // wykorzystanie funkcji odczytu listy komisji obwodowych
                // przypisanych do uzytkownika

                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = user.LoginName;
                headers["X-OPW-token"] = user.AuthenticationToken;
                var checkUri = new Uri(String.Format(URI_COMMISSIONS, user.UserID));
                var checkTask = GetResponse(checkUri, headers);
                checkTask.Wait();

                return true;
            }
            catch
            {
                return false;
            }
        }

        bool IAuthenticationProvider.PasswordRemind(string userName, string email)
        {
            try
            {
                // TODO
                throw new NotImplementedException();

                /*
                var remindRequest = new RemindRequestData()
                {
                    UserName = userName,
                    Email = email
                };
                var remindUri = new Uri(URI_REMIND);
                var remindTask = GetResponse(remindUri, null,
                    JsonHelper.ToJson<RemindRequestData>(remindRequest));
                remindTask.Wait();

                return true;
                */ 
            }
            catch
            {
                return false;
            }
        }

        bool IAuthenticationProvider.UserRegister(string firstName, string lastName, string email)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-API-client"] = API_CLIENT;
                headers["X-OPW-API-token"] = API_TOKEN;
                var registerRequest = new RegisterRequestData()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    Phone = null
                    
                };
                var registerUri = new Uri(URI_REGISTER);
                var registerTask = GetResponse(registerUri, null,
                    JsonHelper.ToJson<RegisterRequestData>(registerRequest));
                registerTask.Wait();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Implementacja IAuthenticationProvider
    }
}
