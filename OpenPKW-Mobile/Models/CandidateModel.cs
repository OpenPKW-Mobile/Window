using OpenPKW_Mobile.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPKW_Mobile.Models
{
    /// <summary>
    /// Model kandydata w wyborach.
    /// </summary>
    public class CandidateModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Pozycja na liście kandydatów.
        /// </summary>
        private int _position = default(int);
        public int Position
        {
            get
            {
                return this._position;
            }
            set
            {
                this._position = value;
                OnPropertyChanged("Position");
            }
        }

        /// <summary>
        /// Dane kandydata.
        /// </summary>
        private CandidateEntity _candidate = null;
        public CandidateEntity Candidate 
        { 
            get
            {
                return this._candidate;
            }
            set
            {
                this._candidate = value;
                OnPropertyChanged("Candidate");
            }
        }

        /// <summary>
        /// Liczba otrzymanych głosów.
        /// </summary>
        private ValueEntry _votes = null;
        public ValueEntry Votes
        {
            get
            {
                return this._votes;
            }
            set
            {
                this._votes = value;
                OnPropertyChanged("Votes");
            }
        }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        public CandidateModel()
        {
        }

        /// <summary>
        /// Konstruktor. 
        /// </summary>
        /// <param name="position"></param>
        /// <param name="candidate"></param>
        public CandidateModel(int position, CandidateEntity candidate)
        {
            this._position = position;
            this._candidate = candidate;
            this._votes = candidate.Votes.HasValue ? new ValueEntry(candidate.Votes.Value) : new ValueEntry();
        }

        #region Implementacja INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion Implementacja INotifyPropertyChanged
    }
}
