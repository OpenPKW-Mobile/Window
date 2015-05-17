using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Providers
{
    class DevElectionProvider : IElectionProvider
    {
        CandidateEntity[] IElectionProvider.GetCandidates()
        {
            throw new NotImplementedException();
        }

        bool IElectionProvider.SendResults(ElectionEntity election)
        {
            throw new NotImplementedException();
        }
    }
}
