#define EMRON

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OpenPKW_Mobile.Resources;

namespace OpenPKW_Mobile
{
    /// <summary>
    /// Strona wprowadzająca aplikacji
    /// </summary>
    public partial class IntroPage : PhoneApplicationPage
    {
        // Constructor
        public IntroPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        /// <summary>
        /// Obsługa przycisku "Dalej"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            if (checkBoxTermsOfUse.IsChecked == true)
            {
                NavigationService.Navigate(new Uri("/LoginPage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show(AppResources.IntroPage_AcceptTermsInfo);
            }
        }

#if EMRON
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //          NavigationService.Navigate(new Uri("/UploadDataPage.xaml", UriKind.Relative));
            //NavigationService.Navigate(new Uri("/EnterVotesPage.xaml", UriKind.Relative));
            
        }
#endif
    }
}