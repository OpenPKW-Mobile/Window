using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Models;
using System.Windows.Data;
using System.Windows.Media;
using OpenPKW_Mobile.Services;
using System.ComponentModel;
using OpenPKW_Mobile.Controls;

namespace OpenPKW_Mobile
{
    public partial class EnterVotesPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Stan strony.
        /// </summary>
        private PageState _state = default(PageState);
        public PageState PageState
        {
            get
            {
                return this._state;
            }
            set
            {
                this._state = value;
                OnPropertyChanged("PageState");
            }
        }

        /// <summary>
        /// Dane komisji wyborczej.
        /// </summary>
        public CommissionEntity Commission
        {
            get
            {
                return App.CurrentAppData.CurrentCommision;
            }
        }

        /// <summary>
        /// Model urny wyborczej.
        /// </summary>
        private BallotBoxModel _ballotBox = null;
        public BallotBoxModel BallotBox
        {
            get
            {
                return this._ballotBox;
            }
            set
            {
                this._ballotBox = value;
                OnPropertyChanged("BallotBox");
            }
        }

        /// <summary>
        /// Lista modeli kandydatów.
        /// </summary>
        private CandidateModel[] _candidates = null;
        public CandidateModel[] Candidates
        {
            get
            {
                return this._candidates;
            }
            set
            {
                this._candidates = value;
                OnPropertyChanged("Candidates");
            }
        }

        /// <summary>
        /// Informacja dla użytkownika o błędzie logowania.
        /// Wyświetla się na dole strony.
        /// </summary>
        protected string Message
        {
            get
            {
                return labelMessage.Text;
            }
            set
            {
                if (value == null)
                {
                    panelWarning.Visibility = Visibility.Collapsed;
                }
                else
                {
                    labelMessage.Text = value;
                    panelWarning.Visibility = Visibility.Visible;
                }
            }
        }

        /// <summary>
        /// Określa, czy strona jest zablokowana do edycji.
        /// Blokowanie edycji następuje w momencie komunikacji z serwerem.
        /// </summary>
        private bool _locked = false;
        protected bool Locked
        {
            get
            {
                return this._locked;
            }
            set
            {
                layoutWait.Visibility = value ? Visibility.Visible : Visibility.Collapsed;
                this._locked = value;
            }
        }

        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        public EnterVotesPage()
        {
            InitializeComponent();

            // podpięcie się pod zdarzenia z serwisu logowania
            // aplikacja będzie mogła reagować w przypadku błędu lub poprawnego uwierzytelniania
            IVotingService service = ServiceManager.Instance.Voting;
            service.FetchCompleted += service_FetchCompleted;
            service.FetchRejected += service_FetchRejected;

            BallotBox = new BallotBoxModel();
            BallotBox.PropertyChanged += BallotBox_PropertyChanged;
        }

        #region Obsługa zdarzeń generowanych przez użytkownika

        /// <summary>
        /// Obsługa zmiany wartości w parametrach urny wyborczej.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BallotBox_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            Message = getUserMessage(PageState = detectCurrentState());
        }

        /// <summary>
        /// Obsługa zmiany wartości w parametrach kandydatów.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Candidates_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            int votes = Candidates
                .Sum(candidate => candidate.Votes);

            BallotBox.ValidVotes.Hints["Candidates"] = new ValueEntry.Hint() { Minimum = votes, Maximum = votes };
            BallotBox.OnPropertyChanged("ValidVotes");

            Message = getUserMessage(PageState = detectCurrentState());
        }

        /// <summary>
        /// Obsługa żądania zmiany komisji
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonCommissionChange_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Wybór komisji", "Zmiana strony", MessageBoxButton.OK);

            // TODO
        }

        /// <summary>
        /// Obsługa żądania przejścia do strony wykonywania zdjęć.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            // zapamiętanie danych
            App.CurrentAppData.Election = BallotBox.GetResults();
            App.CurrentAppData.Candidates = Candidates.Select(model => { model.Candidate.Votes = model.Votes; return model.Candidate; }).ToArray();

            navigateToTakePhotoPage();

            //MessageBox.Show("Wykonanie zdjęć protokołów", "Zmiana strony", MessageBoxButton.OK);

            // TODO
        }

        /// <summary>
        /// Obsługa żądania pominięcia wypełniania formularza.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSkip_Click(object sender, RoutedEventArgs e)
        {
            CandidateModel[] models = null;
            IEnumerable<object> x = models.AsEnumerable();

            MessageBoxEx messageBox = new MessageBoxEx()
            {
                Caption = "Chcesz pominąć wprowadzanie danych ?",
                Message = "Dane ...",
                LeftButtonContent = "Tak",
                RightButtonContent = "Nie",
                Style = (Style)Application.Current.Resources["MessageBoxExStyle"],
            };
            messageBox.LeftButtonPressed += delegate
            {
                //MessageBox.Show("Wykonanie zdjęć protokołów", "Zmiana strony", MessageBoxButton.OK);
                navigateToTakePhotoPage();
                // TODO
            };
            messageBox.Show();
        }

        /// <summary>
        /// Nawigacja do strony wykonywania zdjęć
        /// </summary>
        private void navigateToTakePhotoPage()
        {
            NavigationService.Navigate(new Uri("/TakePhotoPage.xaml?commissionId=2", UriKind.Relative));
        }

        /// <summary>
        /// Obsługa przeniesienia nawigacji do bieżącej strony.
        /// Próba pobrania listy kandydatów.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            // aplikacja spróbuje zalogować się przy użyciu zapamiętanego tokena
            // nazwa użytkownik i hasło powinny być puste
            IVotingService service = ServiceManager.Instance.Voting;

            try
            {
                Locked = true;

                // rozpoczęcie procedury logowania użytkownika w tle
                // koniec procedury jest sygnalizowany poprzez zdarzenia
                service.BeginFetchCandidates();
            }
            catch (LoginException ex)
            {
                Locked = false;
                Message = ex.Message;
            }

            // tutaj w tle trwa procedura logowania
            // ...
        }

        #endregion Obsługa zdarzeń generowanych przez użytkownika

        #region Obsługa zdarzeń z serwisu głosowania

        /// <summary>
        /// Obsługa niepoprawnego wyniku procedury uwierzytelniania.
        /// </summary>
        /// <param name="message"></param>
        void service_FetchRejected(string message)
        {
            Locked = false;
            Message = getUserMessage(PageState = PageState.Fail) ?? message;
            
            Candidates = new CandidateModel[0];
        }

        /// <summary>
        /// Obsługa poprawnego wyniku procedury uwierzytelniania.
        /// </summary>
        /// <param name="user"></param>
        void service_FetchCompleted(CandidateEntity[] candidates)
        {
            Locked = false;
            Message = null;
            
            if (candidates == null)
            {
                Candidates = new CandidateModel[0];
            }
            else
            {
                var models = new List<CandidateModel>();
                foreach (var candidate in candidates)
                {
                    var position = Array.IndexOf(candidates, candidate) + 1;
                    var model = new CandidateModel(position, candidate);
                    model.PropertyChanged += Candidates_PropertyChanged;
                    models.Add(model);
                }
                Candidates = models.ToArray();
            }
        }

        #endregion Obsługa zdarzeń z serwisu głosowania

        #region Implementacja INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion Implementacja INotifyPropertyChanged

        #region Funkcje pomocnicze

        /// <summary>
        /// Określa komunikat dla użytkownika w zależności od stanu strony.
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        private string getUserMessage(PageState state)
        {
            string message = null;

            const string errorMessage = "Dane nie są poprawne. Porównaj wprowadzone wartości z Protokołem Wyborczym.";
            const string readyMessage = "Wprowadziłeś poprawnie dane z Protokołu Wyborczego. Aby zatwierdzić naciśnij DALEJ.";

            switch(state)
            {
                case PageState.Error: message = errorMessage; break;
                case PageState.Ready: message = readyMessage; break;
            }

            return message;
        }

        /// <summary>
        /// Określa stan strony w zależności od stopnia uzupełnienia formularza.
        /// </summary>
        /// <returns></returns>
        private PageState detectCurrentState()
        {
            bool fail = (PageState == PageState.Fail);
            bool dirty =
                BallotBox.IsDirty ||
                Candidates.Any(candidate => candidate.Votes.IsDirty);
            bool valid =
                BallotBox.IsValid &&
                Candidates.All(candidate => candidate.Votes.IsValid);

            PageState state = fail ? PageState.Fail : (dirty ? PageState.Edit : (valid ? PageState.Ready : PageState.Error));

            return state;
        }

        #endregion


    }
}