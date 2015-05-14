using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace OpenPKW_Mobile.Backend.Services
{
    [ServiceContract]
    public interface IStorageService
    {
        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "prepare", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        [return: MessageParameter(Name = "uploadUrl")]
        Uri Prepare(string commissionID, string fileName);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "upload?id={imageID}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Wrapped)]
        void Upload(string imageID, Stream stream);
    }
}
