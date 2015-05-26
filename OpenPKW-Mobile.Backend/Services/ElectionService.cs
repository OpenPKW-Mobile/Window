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
    public class ElectionService : ServiceBase, IElectionService
    {
        const string COMMISSION_ACCEPT = "Commission.Accept";
        const string COMMISSION_VALID = "Commission.Valid";
        const string COMMISSION_ERROR = "Commission.Error";
        const string UPLOAD_ACCEPT = "Upload.Accept";
        const string UPLOAD_VALID = "Upload.Valid";
        const string UPLOAD_ERROR = "Upload.Error";

        public ElectionService()
        {
            AddParam(COMMISSION_ACCEPT, VALUE_YES);
            AddParam(COMMISSION_VALID, VALUE_YES);
            AddParam(COMMISSION_ERROR, VALUE_NO);
            AddParam(UPLOAD_ACCEPT, VALUE_YES);
            AddParam(UPLOAD_VALID, VALUE_YES);
            AddParam(UPLOAD_ERROR, VALUE_NO);
        }

        #region Implementacja IElectionService

        public CommissionDto GetCommission(string pkwID)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string sessionToken = Helper.GetRequestHeader("X-OPW-token");

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(COMMISSION_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
                return null;
            }
            // użytkownik musi posługiwać się prawidłowym tokenem
            // błąd 401
            else if (sessionToken != SESSION_TOKEN)
            {
                Helper.SetResponseStatus(HttpStatusCode.Unauthorized);
                return null;
            }
            // czy komisja jest zarejestrowana w systemie?
            // błąd 404
            else if (GetParam(COMMISSION_VALID) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
                return null;
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(COMMISSION_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
                return null;
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
                return new CommissionDto()
                {
                    pkwId = "106101-4",
                    name = "XV Liceum Ogólnokształcące im.Jana Kasprowicza",
                    address = "ul. Traktorowa 77, 91-204 Łódź",
                    district = new DistrictDto()
                    {
                        pkwId = "14",
                        name = "Okręgowa Komisja Wyborcza Nr 14 w Łodzi"
                    },
                    candidates = new List<CandidateDto>()
                    {
                        new CandidateDto()
                        {
                            pkwId = "1",
                            firstname = "Janusz Ryszard",
                            lastname = "Korwin-Mikke",
                        },
                        new CandidateDto()
                        {
                            pkwId = "2",
                            firstname = "Bronisław Maria",
                            lastname = "Komorowski",                            
                        }
                    }
                };
            }
        }

        public void Upload(string pkwId, ResultDto result)
        {
            Console.WriteLine(Helper.GetCurrentRequest());
            Thread.Sleep(1000);

            string userName = Helper.GetRequestHeader("X-OPW-login");
            string sessionToken = Helper.GetRequestHeader("X-OPW-token");

            // czy serwer może obsłużyć żądanie?
            // błąd 406
            if (GetParam(UPLOAD_ACCEPT) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotAcceptable);
            }
            // użytkownik musi posługiwać się prawidłowym tokenem
            // błąd 401
            else if (sessionToken != SESSION_TOKEN)
            {
                Helper.SetResponseStatus(HttpStatusCode.Unauthorized);
            }
            // czy komisja jest zarejestrowana w systemie?
            // błąd 404
            else if (GetParam(UPLOAD_VALID) == VALUE_NO)
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
            }
            // czy nastąpił błąd podczas wykonywania żądania?
            // błąd 500
            else if (GetParam(UPLOAD_ERROR) == VALUE_YES)
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
        }

        #endregion Implementacja IElectionService
    }
}
