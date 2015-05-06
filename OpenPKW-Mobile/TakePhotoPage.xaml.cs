using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Tasks;
using OpenPKW_Mobile.Models;
using System.Windows.Media.Imaging;

namespace OpenPKW_Mobile
{
    /// <summary>
    /// Strona wykonywania i akceptacji zdjęć protokołu
    /// </summary>
    /// <remarks>
    /// Id komisji przekazywane jest w parametrze commissionId Uri.
    /// Wykonane i zatwierdzone zdjęcia przekazywane są do App.CurrentAppData.ProtocolPages
    /// </remarks>
    public partial class TakePhotoPage : PhoneApplicationPage
    {
        /// <summary>
        /// Id komisji dla której wykonujemy zdjęcia protokołów
        /// </summary>
        int _commissionId;

        /// <summary>
        /// Wykonane i zaakceptowane zdjęcia / strony protokołu
        /// </summary>
        IList<ProtocolPage> _protocolPages;


        CameraCaptureTask _cameraCaptureTask = new CameraCaptureTask();

        /// <summary>
        /// Inicjalizacja
        /// </summary>
        public TakePhotoPage()
        {
            InitializeComponent();
            _cameraCaptureTask.Completed += cameraCaptureTask_Completed;
        }


        /// <summary>
        /// Obsługa zdarzenia wyjścia z zadania wykonywania zdjęcia 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void cameraCaptureTask_Completed(object sender, PhotoResult e)
        {
            if (e.TaskResult == TaskResult.OK)
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.SetSource(e.ChosenPhoto);
                image.Source = bitmapImage;
                setState(TakePhotoPageState.Accept);
            }
        }

        /// <summary>
        /// Ustawienie stanu strony: robienie zdjęcia lub akceptacja
        /// </summary>
        /// <param name="state"></param>
        private void setState(TakePhotoPageState state)
        {
            if (state == TakePhotoPageState.Accept)
            {
                buttonFinish.Visibility = System.Windows.Visibility.Collapsed;
                buttonTakePhoto.Visibility = System.Windows.Visibility.Collapsed;
                buttonRetake.Visibility = System.Windows.Visibility.Visible;
                buttonAccept.Visibility = System.Windows.Visibility.Visible;
                textBlockInfo.Text = "Czy zdjęcie jest wyraźne i dobrze wykadrowane? Jeśli nie ,ponów próbę";
            }
            else if (state == TakePhotoPageState.TakePhoto)
            {
                buttonFinish.Visibility = System.Windows.Visibility.Visible;
                buttonTakePhoto.Visibility = System.Windows.Visibility.Visible;
                buttonRetake.Visibility = System.Windows.Visibility.Collapsed;
                buttonAccept.Visibility = System.Windows.Visibility.Collapsed;
                textBlockInfo.Text = "Zrób zdjęcie każdej pojedynczej i kolejenej stronie PROTOKOŁU WYBORCZEGO";
            }
            else
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Obsługa przycisku wykonania zdjęcia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            _cameraCaptureTask.Show();
        }



        /// <summary>
        /// Obsługa zdarzenia nawigacji do strony
        /// </summary>
        /// <param name="e"></param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _commissionId = int.Parse(NavigationContext.QueryString["commissionId"]);
            textBlockCommissionId.Text = _commissionId.ToString();
            if (!App.CurrentAppData.ProtocolPages.ContainsKey(_commissionId))
            {
                App.CurrentAppData.ProtocolPages.Add(_commissionId, new List<ProtocolPage>());
            }
            _protocolPages = App.CurrentAppData.ProtocolPages[_commissionId];
            base.OnNavigatedFrom(e);
        }

        /// <summary>
        /// Obsługa zdarzenia załadowania strony
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            setState(TakePhotoPageState.TakePhoto);
        }

        /// <summary>
        /// Obsługa przycisku ponownego wykonania zdjęcia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonRetake_Click(object sender, RoutedEventArgs e)
        {
            setState(TakePhotoPageState.TakePhoto);
        }

        /// <summary>
        /// Obsługa przycisku akceptacji zdjęcia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonAccept_Click(object sender, RoutedEventArgs e)
        {
            ProtocolPage pageImage = new ProtocolPage();
            pageImage.Image = image.Source as BitmapImage;
            _protocolPages.Add(pageImage);
            setState(TakePhotoPageState.TakePhoto);
        }

        /// <summary>
        /// Stan strony
        /// </summary>
        enum TakePhotoPageState
        {
            Accept,
            TakePhoto
        }

        private void buttonFinish_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/PreviewDataPage.xaml", UriKind.Relative));            
        }
    }
}