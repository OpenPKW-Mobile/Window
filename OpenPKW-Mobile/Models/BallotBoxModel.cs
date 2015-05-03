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
    /// Model urny wyborczej.
    /// </summary>
    public class BallotBoxModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Liczba uprawnionych do głosowania.
        /// </summary>
        private ValueEntry _voters = new ValueEntry();
        public ValueEntry Voters
        {
            get
            {
                return this._voters;
            }
            set
            {
                var previous = this._voters;
                var current = value;
                if (current != previous)
                {
                    this._voters = value;
                    this._voters.Hints = previous.Hints;
                    OnVotersChanged(current, previous);
                }
            }
        }

        /// <summary>
        /// Liczba wydanych kart do głosowania.
        /// </summary>
        private ValueEntry _cards = new ValueEntry();
        public ValueEntry Cards
        {
            get
            {
                return this._cards;
            }
            set
            {
                var previous = this._cards;
                var current = value;
                if (current != previous)
                {
                    this._cards = value;
                    this._cards.Hints = previous.Hints;
                    OnCardsChanged(current, previous);
                }
            }
        }

        /// <summary>
        /// Liczba ważnych kart do głosowania.
        /// </summary>
        private ValueEntry _validCards = new ValueEntry();
        public ValueEntry ValidCards
        {
            get
            {
                return this._validCards;
            }
            set
            {
                var previous = this._validCards;
                var current = value;
                if (current != previous)
                {
                    this._validCards = value;
                    this._validCards.Hints = previous.Hints;
                    OnValidCardsChanged(current, previous);
                }
            }
        }

        /// <summary>
        /// Liczba ważnych głosów.
        /// </summary>
        private ValueEntry _validVotes = new ValueEntry();
        public ValueEntry ValidVotes
        {
            get
            {
                return this._validVotes;
            }
            set
            {
                var previous = this._validVotes;
                var current = value;
                if (current != previous)
                {
                    this._validVotes = value;
                    this._validVotes.Hints = previous.Hints;
                    OnValidVotesChanged(current, previous);
                }
            }
        }

        /// <summary>
        /// Liczba nieważnych głosów.
        /// </summary>
        private ValueEntry _invalidVotes = new ValueEntry();
        public ValueEntry InvalidVotes
        {
            get
            {
                return this._invalidVotes;
            }
            set
            {
                var previous = this._invalidVotes;
                var current = value;
                if (current != previous)
                {
                    this._invalidVotes = value;
                    this._invalidVotes.Hints = previous.Hints;
                    OnInvalidVotesChanged(current, previous);
                }
            }
        }

        /// <summary>
        /// Czy wszystkie elementy są prawidłowe?
        /// </summary>
        public bool IsValid
        {
            get
            {
                return
                    Voters.IsValid &&
                    Cards.IsValid &&
                    ValidCards.IsValid &&
                    ValidVotes.IsValid &&
                    InvalidVotes.IsValid;
            }
        }

        /// <summary>
        /// Czy są elementy nieuzupełnione lub z błędnymi wartościami?
        /// </summary>
        public bool IsDirty
        {
            get
            {
                return
                    Voters.IsDirty ||
                    Cards.IsDirty ||
                    ValidCards.IsDirty ||
                    ValidVotes.IsDirty ||
                    InvalidVotes.IsDirty;
            }
        }

        /// <summary>
        /// Obsługa zmiany liczby uprawnionych do głosowania.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        protected virtual void OnVotersChanged(ValueEntry current, ValueEntry previous)
        {
            if (current != null)
            {
                Cards.Hints["Voters"] = new ValueEntry.Hint() { Minimum = 0, Maximum = current };
                OnPropertyChanged("Cards");
            }

            OnPropertyChanged("Voters");
        }

        /// <summary>
        /// Obsługa zmiany liczby wydanych kart do głosowania.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        protected virtual void OnCardsChanged(ValueEntry current, ValueEntry previous)
        {
            if (current != null)
            {
                Voters.Hints["Cards"] = new ValueEntry.Hint() { Minimum = current, Maximum = int.MaxValue };
                ValidCards.Hints["Cards"] = new ValueEntry.Hint() { Minimum = 0, Maximum = current };
                OnPropertyChanged("Voters");
                OnPropertyChanged("ValidCards");
            }
            
            OnPropertyChanged("Cards");
        }

        /// <summary>
        /// Obsługa zmiany liczby ważnych kart do głosowania.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        protected virtual void OnValidCardsChanged(ValueEntry current, ValueEntry previous)
        {
            if (current != null)
            {
                Cards.Hints["ValidCards"] = new ValueEntry.Hint() { Minimum = current, Maximum = int.MaxValue };
                InvalidVotes.Hints["ValidCards"] = new ValueEntry.Hint() { Minimum = 0, Maximum = current };
                ValidVotes.Hints["ValidCards"] = new ValueEntry.Hint() { Minimum = 0, Maximum = current };
                OnPropertyChanged("Cards");
                OnPropertyChanged("InvalidVotes");
                OnPropertyChanged("ValidVotes");
            }

            OnPropertyChanged("ValidCards");
        }

        /// <summary>
        /// Obsługa zmiany liczby nieważnych głosów.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        protected virtual void OnInvalidVotesChanged(ValueEntry current, ValueEntry previous)
        {
            if (current != null)
            {
                if (ValidVotes != null && !ValidVotes.IsDirty)
                {
                    ValidCards.Hints["...Votes"] = new ValueEntry.Hint() { Minimum = current + ValidVotes, Maximum = current + ValidVotes };
                    OnPropertyChanged("ValidCards");
                }
            }

            OnPropertyChanged("InvalidVotes");
        }

        /// <summary>
        /// Obsługa zmiany liczby ważnych głosów.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        protected virtual void OnValidVotesChanged(ValueEntry current, ValueEntry previous)
        {
            if (current != null)
            {
                if (InvalidVotes != null && !InvalidVotes.IsDirty)
                {
                    ValidCards.Hints["...Votes"] = new ValueEntry.Hint() { Minimum = current + InvalidVotes, Maximum = current + InvalidVotes };
                    OnPropertyChanged("ValidCards");
                }
            }

            OnPropertyChanged("ValidVotes");
        }

        /// <summary>
        /// Pobranie wyniku.
        /// </summary>
        /// <returns></returns>
        public ElectionEntity GetResults()
        {
            if (!IsValid)
                return null;

            ElectionEntity election = new ElectionEntity()
            {
                Voters = this.Voters.Value,
                Cards = this.Cards.Value,
                ValidCards = this.ValidCards.Value,
                ValidVotes = this.ValidVotes.Value,
                InvalidVotes = this.InvalidVotes.Value
            };

            return election;
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
