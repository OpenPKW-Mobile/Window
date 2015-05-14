using OpenPKW_Mobile.Backend.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OpenPKW_Mobile.Backend.Services
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "login", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        UserDto Login();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "logout", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void Logout();

        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "check", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void Check();

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "remind", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Remind(string name, string email);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "register", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Register(string firstname, string lastname, string email);
    }   
}
