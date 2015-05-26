using OpenPKW_Mobile.Backend.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading;

namespace OpenPKW_Mobile.Backend.Services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class AuthenticationService : ServiceBase, IAuthenticationService
    {
        const string LOGIN_ACCEPT = "Login.Accept";
        const string LOGIN_VALID = "Login.Valid";
        const string LOGIN_ERROR = "Login.Error";
        const string LOGOUT_ACCEPT = "Logout.Accept";
        const string LOGOUT_ERROR = "Logout.Error";
        const string CHECK_ACCEPT = "Check.Accept";
        const string CHECK_ERROR = "Check.Error";
        const string CHECK_ACTIVE = "Check.Active";
        const string REMIND_ACCEPT = "Remind.Accept";
        const string REMIND_VALID = "Remind.Valid";
        const string REMIND_ERROR = "Remind.Error";
        const string REGISTER_ACCEPT = "Register.Accept";
        const string REGISTER_VALID = "Register.Valid";
        const string REGISTER_ERROR = "Register.Error";

        public AuthenticationService()
        {
            AddParam(LOGIN_ACCEPT, VALUE_YES);
            AddParam(LOGIN_VALID, VALUE_YES);
            AddParam(LOGIN_ERROR, VALUE_NO);
            AddParam(LOGOUT_ACCEPT, VALUE_YES);
            AddParam(LOGOUT_ERROR, VALUE_NO);
            AddParam(CHECK_ACCEPT, VALUE_YES);
            AddParam(CHECK_ERROR, VALUE_NO);
            AddParam(REMIND_ACCEPT, VALUE_YES);
            AddParam(REMIND_VALID, VALUE_YES);
            AddParam(REMIND_ERROR, VALUE_NO);
            AddParam(REGISTER_ACCEPT, VALUE_YES);
            AddParam(REGISTER_VALID, VALUE_YES);
            AddParam(REGISTER_ERROR, VALUE_NO);
        }

        #region Implementacja IAuthenticationService

        public UserDto Login()
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string userPassword = Helper.GetRequestHeader("X-OPW-password");

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(LOGIN_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
                return null;
            }
            // czy użytkownik podał właściwe poświadczenia?
            // błąd 401
            else if (GetParam(LOGIN_VALID) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.Unauthorized);
                return null;
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(LOGIN_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
                return null;
            }
            else
            {
                DateTime timeout = DateTime.Now + TimeSpan.FromHours(1);
                UserDto user = new UserDto()
                {
                    id = 1,
                    fullname = "Jan Kowalski",
                    firstname = "Jan",
                    lastname = "Kowalski",
                    login = userName,
                    token = SESSION_TOKEN,
                    sessionActive = true,
                    sessionTimeout = (timeout.Ticks / TimeSpan.TicksPerSecond).ToString()
                };

                Helper.SetResponseStatus(HttpStatusCode.OK);
                return user;
            }
        }

        public void Logout()
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string sessionToken = Helper.GetRequestHeader("X-OPW-token");

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(LOGOUT_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
            }
            // użytkownik musi posługiwać się prawidłowym tokenem
            // błąd 401
            else if (sessionToken != SESSION_TOKEN)
            {
                Helper.SetResponseStatus(HttpStatusCode.Unauthorized);
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(LOGOUT_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
        }

        public void Check()
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string sessionToken = Helper.GetRequestHeader("X-OPW-token");

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(CHECK_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
            }
            // użytkownik musi posługiwać się prawidłowym tokenem
            // błąd 401
            else if (sessionToken != SESSION_TOKEN)
            {
                Helper.SetResponseStatus(HttpStatusCode.Unauthorized);
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(CHECK_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
            // czy sesja użytkownika jest aktywna
            // błąd 404
            else if (GetParam(CHECK_ACTIVE) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
        }

        public void Remind(string name, string email)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(REMIND_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(REMIND_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
            // czy użytkownik jest zarejestrowany w systemie?
            // błąd 404
            else if (GetParam(REMIND_VALID) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
        }

        public void Register(string firstName, string lastName, string email)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(REGISTER_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
            }
            // czy użytkownik jest zarejestrowany w systemie?
            // błąd 302
            if (GetParam(REGISTER_VALID) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.Found);
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(REGISTER_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
        }

        #endregion Implementacja IAuthenticationService
    }

   
}
