using OpenPKW_Mobile.Backend.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace OpenPKW_Mobile.Backend.Services
{
    public class AuthenticationService : IAuthenticationService
    {       
        public UserDto Login()
        {
            Console.WriteLine(Helper.GetCurrentRequest());

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string userPassword = Helper.GetRequestHeader("X-OPW-password");

            if (userName == "a" && userPassword == "b")
            {
                DateTime timeout = DateTime.Now + TimeSpan.FromHours(1);
                UserDto user = new UserDto()
                {
                    id = 1,
                    fullname = "Jan Kowalski",
                    firstname = "Jan",
                    lastname = "Kowalski",
                    login = "emron",
                    token = "1234567890",
                    sessionActive = true,
                    sessionTimeout = (timeout.Ticks / TimeSpan.TicksPerSecond).ToString()
                };

                Helper.SetResponseStatus(HttpStatusCode.OK);
                return user;
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
                return null;
            }
        }

        public void Logout()
        {
            Console.WriteLine(Helper.GetCurrentRequest());

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string sessionToken = Helper.GetRequestHeader("X-OPW-token");

            if (userName == "a" && sessionToken == "1234567890")
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
            }
        }

        public void Check()
        {
            Console.WriteLine(Helper.GetCurrentRequest());

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string sessionToken = Helper.GetRequestHeader("X-OPW-token");

            if (userName == "a" && sessionToken == "1234567890")
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
            }
        }

        public void Remind(string name, string email)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
        }

        public void Register(string firstname, string lastname, string email)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
        }
    }

   
}
