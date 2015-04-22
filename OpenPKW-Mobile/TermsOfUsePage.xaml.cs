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
    /// Strona z regulaminem i zasadami korzystania z programu
    /// </summary>
    public partial class TermsOfUsePage : PhoneApplicationPage
    {
        /// <summary>
        /// Url do strony z regulaminem
        /// </summary>
        private const string termOfUseUrl = "http://openpkw.pl"; //TODO: ustawić poprawny adres

        /// <summary>
        /// Inicjalizacja
        /// </summary>
        public TermsOfUsePage()
        {
            InitializeComponent();
           
        }

        /// <summary>
        /// Obsługa zdarzenia błędu nawigacji przeglądarki
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webBrowser_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            removeProgress();
        }


        /// <summary>
        /// Obsługa zdarzenia poprawnego załadowania strony w webrowser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void webBrowser_Navigated(object sender, NavigationEventArgs e)
        {
            removeProgress();
        }

        /// <summary>
        /// Usunięcie paska postępu
        /// </summary>
        private void removeProgress()
        {
            gridMain.Children.Remove(progressBar);
        }

        /// <summary>
        /// Obsługa zdarzenia załadowania strony
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            webBrowser.Navigate(new Uri(termOfUseUrl));
        }

    }
}