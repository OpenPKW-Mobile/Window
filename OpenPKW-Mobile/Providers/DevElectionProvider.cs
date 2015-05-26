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
    class DevElectionProvider : ProviderBase, IElectionProvider
    {
        const string URI_CANDIDATES = "http://169.254.80.80:8733/Design_Time_Addresses/election/commission/{0}";
        const string URI_COMMISSION = "http://169.254.80.80:8733/Design_Time_Addresses/election/commission/{0}";
        const string URI_UPLOAD = "http://169.254.80.80:8733/Design_Time_Addresses/election/upload/{0}";

        #region Komunikaty

        [DataContract]
        public class CommissionResponseData
        {
            [DataContract]
            public class DistrictDto
            {
                [DataMember(Name = "pkwId")]
                public String CommissionID { get; set; }

                [DataMember(Name = "name")]
                public String Name { get; set; }

                [DataMember(Name = "address")]
                public String Address { get; set; }
            }

            [DataContract]
            public class CandidateDto
            {
                [DataMember(Name = "pkwId")]
                public string pkwId { get; set; }

                [DataMember(Name = "firstname")]
                public String firstname { get; set; }

                [DataMember(Name = "lastname")]
                public String lastname { get; set; }

                [DataMember(Name = "votes")]
                public long votes { get; set; }
            }

            [DataMember(Name = "pkwId")]
            public String CommissionID { get; set; }

            [DataMember(Name = "name")]
            public String Name { get; set; }

            [DataMember(Name = "address")]
            public String Address { get; set; }

            [DataMember(Name = "district")]
            public DistrictDto District { get; set; }

            [DataMember(Name = "candidates")]
            public List<CandidateDto> Candidates { get; set; }
        }

        [DataContract]
        public class UploadRequestData
        {
            [DataMember(Name = "uprawnionych")]
            public int Voters { get; set; }

            [DataMember(Name = "glosujacych")]
            public int Cards { get; set; }

            [DataMember(Name = "kartWaznych")]
            public int ValidCards { get; set; }

            [DataMember(Name = "glosowNieWaznych")]
            public int InvalidVotes { get; set; }

            [DataMember(Name = "glosowWaznych")]
            public int ValidVotes { get; set; }

            [DataMember(Name = "k1")]
            public int K1 { get; set; }

            [DataMember(Name = "k2")]
            public int K2 { get; set; }

            [DataMember(Name = "k3")]
            public int K3 { get; set; }

            [DataMember(Name = "k4")]
            public int K4 { get; set; }

            [DataMember(Name = "k5")]
            public int K5 { get; set; }

            [DataMember(Name = "k6")]
            public int K6 { get; set; }

            [DataMember(Name = "k7")]
            public int K7 { get; set; }

            [DataMember(Name = "k8")]
            public int K8 { get; set; }

            [DataMember(Name = "k9")]
            public int K9 { get; set; }

            [DataMember(Name = "k10")]
            public int K10 { get; set; }

            [DataMember(Name = "k11")]
            public int K11 { get; set; }
        }

        #endregion Komunikaty

        #region Implementacja IElectionProvider

        CommissionEntity IElectionProvider.GetCommission(UserEntity user, string commissionID)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = user.LoginName;
                headers["X-OPW-token"] = user.AuthenticationToken;
                var commissionUri = new Uri(String.Format(URI_COMMISSION, commissionID));
                var commissionTask = GetResponse(commissionUri, headers);
                commissionTask.Wait();
                var commissionResponse = JsonHelper.FromJson<CommissionResponseData>(commissionTask.Result);

                return new CommissionEntity()
                {
                    Id = commissionResponse.CommissionID,
                    Address = commissionResponse.Address,
                    Place = commissionResponse.Name,
                    Title = null,
                    Type = commissionResponse.District.Name,
                };
            }
            catch
            {
                return null;
            }
        }

        CandidateEntity[] IElectionProvider.GetCandidates(UserEntity user, CommissionEntity commission)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = user.LoginName;
                headers["X-OPW-token"] = user.AuthenticationToken;
                var commissionUri = new Uri(String.Format(URI_CANDIDATES, commission.Id));
                var commissionTask = GetResponse(commissionUri, headers);
                commissionTask.Wait();
                var commissionResponse = JsonHelper.FromJson<CommissionResponseData>(commissionTask.Result);

                var candidates = from candidate in commissionResponse.Candidates
                                 select new CandidateEntity()
                                 {
                                     Id = candidate.pkwId,
                                     FirstName = candidate.firstname,
                                     SecondName = null,
                                     Surname = candidate.lastname,
                                     Votes = null
                                 };

                return candidates.ToArray();
            }
            catch
            {
                return null;
            }
        }

        bool IElectionProvider.UploadResults(UserEntity user, CommissionEntity commission, ElectionEntity election, CandidateEntity[] candidates)
        {
            try
            {
                WebHeaderCollection headers = new WebHeaderCollection();
                headers["X-OPW-login"] = user.LoginName;
                headers["X-OPW-token"] = user.AuthenticationToken;

                var uploadRequest = new UploadRequestData()
                {
                    Voters = election.Voters,
                    Cards = election.Cards,
                    ValidCards = election.ValidCards,
                    InvalidVotes = election.InvalidVotes,
                    ValidVotes = election.ValidVotes,
                    K1 = getCandidateVotes(candidates, 1),
                    K2 = getCandidateVotes(candidates, 2),
                    K3 = getCandidateVotes(candidates, 3),
                    K4 = getCandidateVotes(candidates, 4),
                    K5 = getCandidateVotes(candidates, 5),
                    K6 = getCandidateVotes(candidates, 6),
                    K7 = getCandidateVotes(candidates, 7),
                    K8 = getCandidateVotes(candidates, 8),
                    K9 = getCandidateVotes(candidates, 9),
                    K10 = getCandidateVotes(candidates, 10),
                    K11 = getCandidateVotes(candidates, 11),
                };
                var uploadUri = new Uri(String.Format(URI_UPLOAD, commission.Id));
                var uploadTask = GetResponse(uploadUri, headers,
                    JsonHelper.ToJson<UploadRequestData>(uploadRequest));
                uploadTask.Wait();

                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion Implementacja IElectionProvider

        #region Funkcje pomocnicze

        private int getCandidateVotes(CandidateEntity[] candidates, int candidateID)
        {
            var candidate = candidates.FirstOrDefault(item => item.Identifier == candidateID.ToString());
            return (candidate != null) && candidate.Votes.HasValue 
                ? candidate.Votes.Value : 0;
        }

        #endregion

    }
}
