using OpenPKW_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Mocks
{
    public class DesignBallotBoxModel : BallotBoxModel
    {
        public DesignBallotBoxModel()
        {
            this.Voters = new ValueEntry(3000);
            this.Cards = new ValueEntry(3000);
            this.ValidCards = new ValueEntry(2900);
            this.InvalidVotes = new ValueEntry(10);
            this.ValidVotes = new ValueEntry();

            this.InvalidVotes.Hints["hint"] = new ValueEntry.Hint() { Minimum = 2900, Maximum = 2900 };
        }
    }
}
