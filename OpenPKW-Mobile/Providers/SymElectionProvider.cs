using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace OpenPKW_Mobile.Providers
{
    class SymElectionProvider : ProviderBase, IElectionProvider
    {     
        CommissionEntity IElectionProvider.GetCommission(UserEntity user, string commissionID)
        {
            throw new NotImplementedException();
        }

        CandidateEntity[] IElectionProvider.GetCandidates(UserEntity user, CommissionEntity commission)
        {
            var message = "Czy wysłać listę kandydatów w wyborach ?";
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            if (result == MessageBoxResult.Cancel)
                return null;
            else
            {
                return new CandidateEntity[]
                {
                    new CandidateEntity()
                    {
                        Surname = "Korwin-Mikke",
                        FirstName = "Janusz",
                        SecondName = "Ryszard"
                    },
                    new CandidateEntity()
                    {
                        Surname = "Komorowski",
                        FirstName = "Bronisław",
                        SecondName = "Maria"
                    },
                    new CandidateEntity()
                    {
                        Surname = "Kowalewski",
                        FirstName = "Jan",
                        SecondName = "Maria"
                    },
                    new CandidateEntity()
                    {
                        Surname = "Nowak",
                        FirstName = "Tadeusz"
                    },
                    new CandidateEntity()
                    {
                        Surname = "Niewiadomy",
                        FirstName = "Zdzisław"
                    }
                };
            }
        }

        bool IElectionProvider.UploadResults(UserEntity user, CommissionEntity commission, ElectionEntity election, CandidateEntity[] candidates)
        {
            var message = "Czy odebrać dane z wynikami wyborów ?";
            var result = ShowMessage(message);

            // symuluje opóżnienia w komunikacji z zewnętrzną usługą
            Thread.Sleep(3000);

            return (result == MessageBoxResult.OK);
        }
    }
}
