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
                //MessageBox.Show("Strona logowania");
            }
            else
            {
                MessageBox.Show(AppResources.IntroPage_AcceptTermsInfo);
            }
        }


        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}