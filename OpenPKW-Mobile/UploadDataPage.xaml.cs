using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.ComponentModel;
using OpenPKW_Mobile.Entities;
using OpenPKW_Mobile.Controls;
using OpenPKW_Mobile.Services;

namespace OpenPKW_Mobile
{
    public partial class UploadDataPage : PhoneApplicationPage, INotifyPropertyChanged
    {
        /// <summary>
        /// Dane dotyczące postępu operacji.
        /// </summary>
        public struct ProgressData
        {          
            public readonly int Value;
            public readonly string Text;

            public ProgressData(int value, string text)
            {
                this.Value = value;
                this.Text = text;
            }

            public override string ToString()
            {
                return this.Text;
            }

            public override int GetHashCode()
            {
                return this.Value;
            }      
        }

        /// <summary>
        /// Definicja kolejki elementów do wysłania.
        /// </summary>
        protected class UploadQueue : Queue<object>
        {
            public readonly int InitialCount;
            public UploadQueue(IEnumerable<object> collection)
                : base(collection)
            {
                InitialCount = collection.Count();
            }
        }

        /// <summary>
        /// Kolejka elementów do wysłania.
        /// </summary>
        private UploadQueue _queue = null;
        protected UploadQueue Queue
        {
            get
            {
                if (this._queue == null)
                    this._queue = prepareQueue();
                return this._queue;
            }
        }

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
        /// Informacja dla użytkownika o błędzie logowania.
        /// Wyświetla się na dole strony.
        /// </summary>
        private string _message = null;
        public string Message
        {
            get
            {
                return this._message; 
            }
            set
            {
                this._message = value;
                OnPropertyChanged("Message");
            }
        }

        /// <summary>
        /// Informacja dla użytkownika o błędzie logowania.
        /// Wyświetla się na dole strony.
        /// </summary>
        private string _information = null;
        public string Information
        {
            get
            {
                return this._information;
            }
            set
            {
                this._information = value;
                OnPropertyChanged("Information");
            }
        }
        
        /// <summary>
        /// Postęp operacji wysyłania danych.
        /// </summary>
        private ProgressData _progress = default(ProgressData);
        public ProgressData Progress
        {
            get
            {
                return this._progress;
            }
            set
            {
                this._progress = value;
                OnPropertyChanged("Progress");
            }
        }
     
        /// <summary>
        /// Konstruktor klasy.
        /// </summary>
        public UploadDataPage()
        {
            InitializeComponent();

            IVotingService serviceVoting = ServiceManager.Instance.Voting;
            serviceVoting.UploadCompleted += serviceVoting_UploadCompleted;
            serviceVoting.UploadRejected += serviceVoting_UploadRejected;

            IPhotoService servicePhoto = ServiceManager.Instance.Photo;
            servicePhoto.UploadProgress += servicePhoto_UploadProgress;
            servicePhoto.UploadCompleted += servicePhoto_UploadCompleted;
            servicePhoto.UploadRejected += servicePhoto_UploadRejected;
        }

        /// <summary>
        /// Wysłanie następnych danych z kolejki.
        /// </summary>
        protected void UploadNext()
        {
            var queue = this.Queue;

            // brak danych do wysłania
            // należy poinformować użytkownika o zakończeniu operacji
            if (queue.Count == 0)
            {
                PageState = PageState.Ready;
                Message = getUserMessage(PageState);
                Information = getUserInformation(PageState);

                return;
            }

            // są dane do wysłania
            // sposób wysłania zależy od rodzaju danych (zdjęcia, wyniki głosowania)
            try
            {
                PageState = PageState.Wait;
                Message = null;
                Information = null;

                var item = queue.Peek();
                if (item is ElectionEntity)
                {
                    IVotingService service = ServiceManager.Instance.Voting;
                    ElectionEntity entity = (ElectionEntity)item;
                    service.BeginUpload(entity);

                    Progress = new ProgressData(Progress.Value, "Wyniki głosowania");
                }
                else if (item is PhotoEntity)
                {
                    IPhotoService service = ServiceManager.Instance.Photo;
                    CommissionEntity commission = App.CurrentAppData.CurrentCommision;
                    PhotoEntity entity = (PhotoEntity)item;
                    service.BeginUpload(commission, entity);

                    Progress = new ProgressData(Progress.Value, entity.Name);
                }
            }
            catch (LoginException ex)
            {
                PageState = PageState.Fail;
                Message = ex.Message;
            }

            // tutaj w tle trwa procedura wysyłania danych
            // ...
        }

        #region Obsługa zdarzeń generowanych przez użytkownika

        /// <summary>
        /// Obsługa żądania zamknięcia aplikacji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxEx messageBox = new MessageBoxEx()
            {
                Caption = "Chcesz wyjść z aplikacji ?",
                Message = "Wyniki wyborów zostaną opublikowane na e-wybory.org",
                LeftButtonContent = "Tak",
                RightButtonContent = "Nie",
                Style = (Style)Application.Current.Resources["MessageBoxExStyle"],
            };
            messageBox.LeftButtonPressed += delegate
            {
                App.Current.Terminate();
            };
            messageBox.Show();
        }

        /// <summary>
        /// Obsługa żądania ponowienia transmisji.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRetry_Click(object sender, RoutedEventArgs e)
        {
            UploadNext();
        }

        /// <summary>
        /// Obsługa przeniesienia nawigacji do bieżącej strony.
        /// Rozpoczęcie wysyłania danych.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            UploadNext();
        }
        
        #endregion Obsługa zdarzeń generowanych przez użytkownika

        #region Obsługa zdarzeń z serwisu

        /// <summary>
        /// Obsługa poprawnego wysłania danych głosowania.
        /// </summary>
        private void serviceVoting_UploadCompleted()
        {
            Queue.Dequeue();
            UploadNext();
        }

        /// <summary>
        /// Obsługa poprawnego wysłania zdjęć protokołów.
        /// </summary>
        private void servicePhoto_UploadCompleted()
        {
            Queue.Dequeue();
            UploadNext();
        }

        /// <summary>
        /// Obsługa błędu podczas wysyłania danych głosowania.
        /// </summary>
        /// <param name="message"></param>
        private void serviceVoting_UploadRejected(string message)
        {
            PageState = PageState.Error;
            Message = getUserMessage(PageState) ?? message;
            Information = getUserInformation(PageState);
        }

        /// <summary>
        /// Obsługa błędu podczas wysyłania zdjęć protokołów.
        /// </summary>
        /// <param name="message"></param>
        private void servicePhoto_UploadRejected(string message)
        {
            PageState = PageState.Error;
            Message = getUserMessage(PageState) ?? message;
            Information = getUserInformation(PageState);
        }

        /// <summary>
        /// Obsługa zmiany postępu wysyłania danych zdjęć.
        /// </summary>
        /// <param name="value"></param>
        void servicePhoto_UploadProgress(int value)
        {
            value = (int)((100.0 * (Queue.InitialCount - Queue.Count) + value) / Queue.InitialCount);
            value = Math.Max(0, Math.Min(100, value));

            Progress = new ProgressData(value, Progress.Text);
        }

        #endregion Obsługa zdarzeń z serwisu

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
        /// Przytowanie listy danych do wysłania.
        /// </summary>
        /// <returns></returns>
        private UploadQueue prepareQueue()
        {
            List<object> items = new List<object>();
            //            items.Add(App.CurrentAppData.Election);
            foreach (var photo in App.CurrentAppData.ProtocolPhotos)
                items.Add(photo);

            return new UploadQueue(items);
        }

        /// <summary>
        /// Określa komunikat dla użytkownika w zależności od stanu strony.
        /// </summary>
        /// <param name="state">Stan strony</param>
        /// <returns>Komunikat</returns>
        private string getUserMessage(PageState state)
        {
            string message = null;

            const string errorMessage = "";
            const string readyMessage = "Twoje dane zostały poprawnie wysłane.";

            switch (state)
            {
                case PageState.Error: message = errorMessage; break;
                case PageState.Ready: message = readyMessage; break;
            }

            return message;
        }

        /// <summary>
        /// Określa dodatkową informację dla użytkownika w zależności od stanu strony.
        /// </summary>
        /// <param name="state">Stan strony</param>
        /// <returns>Informacja</returns>
        private string getUserInformation(PageState state)
        {
            string message = null;

            const string errorMessage = "Sprawdź, czy jesteś połączony z Internetem i spróbuj ponownie.";
            const string readyMessage = "Dziękujemy za poświęcony czas i dobrze wykonaną misję społeczną.";

            switch (state)
            {
                case PageState.Error: message = errorMessage; break;
                case PageState.Ready: message = readyMessage; break;
            }

            return message;
        }

        #endregion Funkcje pomocnicze
    }
}