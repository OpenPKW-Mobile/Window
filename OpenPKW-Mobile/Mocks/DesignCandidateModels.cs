using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Mocks
{ 
    public class DesignCandidateModels
    {
        public List<CandidateModel> Candidates { get; set; }

        public DesignCandidateModels()
        {
            var items = new CandidateEntity[]
            {
                new CandidateEntity()
                {
                    Surname = "Surname1",
                    FirstName = "FirstName1",
                    SecondName = "SecondName1",
                    Votes = 1234
                },
                new CandidateEntity()
                {
                    Surname = "Surname2",
                    FirstName = "FirstName2",
                    SecondName = "SecondName2"
                },
                new CandidateEntity()
                {
                    Surname = "Surname3",
                    FirstName = "FirstName3",
                    SecondName = "SecondName3"
                },
                new CandidateEntity()
                {
                    Surname = "Surname4",
                    FirstName = "FirstName4",
                    SecondName = "SecondName4"
                },
                new CandidateEntity()
                {
                    Surname = "Surname5",
                    FirstName = "FirstName5",
                    SecondName = "SecondName5"
                }
            };

            var models = from item in items
                         let position = Array.IndexOf(items, item) + 1
                         select new CandidateModel(position, item);

            Candidates = new List<CandidateModel>(models);
        }
    }
}
