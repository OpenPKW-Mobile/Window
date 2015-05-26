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
    class DevAuthenticationProvider : ProviderBase, IAuthenticationProvider
    {
        const string URI_LOGIN = "http://169.254.80.80:8733/Design_Time_Addresses/user/login";
        const string URI_LOGOUT = "http://169.254.80.80:8733/Design_Time_Addresses/user/logout";
        const string URI_CHECK = "http://169.254.80.80:8733/Design_Time_Addresses/user/check";
        const string URI_REMIND = "http://169.254.80.80:8733/Design_Time_Addresses/user/remind";
        const string URI_REGISTER = "http://169.254.80.80:8733/Design_Time_Addresses/user/register";

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
            [DataMember(Name = "firstName")]
            public string FirstName { get; set; }

            [DataMember(Name = "lastName")]
            public string LastName { get; set; }

            [DataMember(Name = "email")]
            public string Email { get; set; }
        }

        #endregion Kominikaty

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
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = user.LoginName;
                headers["X-OPW-token"] = user.AuthenticationToken;
                var checkUri = new Uri(URI_CHECK);
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
                var registerRequest = new RegisterRequestData()
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email
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
