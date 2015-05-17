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
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class ElectionService : ServiceBase, IElectionService
    {
        public CommissionDto GetCommission(string pkwID)
        {
            Console.WriteLine(Helper.GetCurrentRequest());

            if (pkwID == "1")
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
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.NotFound);
                return null;
            }
        }

        public void Upload(string pkwId, ResultDto result)
        {
            Console.WriteLine(Helper.GetCurrentRequest());

            if (pkwId == "1")
            {
                Helper.SetResponseStatus(HttpStatusCode.OK);
            }
            else
            {
                Helper.SetResponseStatus(HttpStatusCode.InternalServerError);
            }
        }
    }
}
