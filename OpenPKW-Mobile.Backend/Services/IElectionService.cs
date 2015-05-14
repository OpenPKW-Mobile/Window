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
    public interface IElectionService
    {
        [OperationContract]
        [WebInvoke(Method = "GET", UriTemplate = "commission/{pkwId}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        CommissionDto GetCommission(string pkwId);

        [OperationContract]
        [WebInvoke(Method = "POST", UriTemplate = "upload/{pkwId}", ResponseFormat = WebMessageFormat.Json, BodyStyle = WebMessageBodyStyle.Bare)]
        void Upload(string pkwId, ResultDto result);
    }
}
