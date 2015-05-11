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
using System.Collections;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using OpenPKW_Mobile.Mocks;
using OpenPKW_Mobile.Models;

namespace OpenPKW_Mobile
{
    public partial class PreviewDataPage : PhoneApplicationPage
    {
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
        /// Lista zdjęć protokołów.
        /// </summary>
        public IEnumerable Photos
        {
            get
            {
                var photos = App.CurrentAppData.ProtocolPhotos;
                var items = from photo in photos
                            select new PhotoModel
                            {
                                Name = photo.Name,
                                Image = photo.Image
                            };
                return items;
            }
        }

        /// <summary>
        /// Proporcje siatki zdjęć.
        /// </summary>
        public Size GridRatio
        {
            get
            {
                var count = App.CurrentAppData.ProtocolPhotos.Count();
                var columns = 2.0;
                var rows = Math.Ceiling(count / columns);
                return new Size(columns, rows);
            }
        }

        /// <summary>
        /// Konstruktor.
        /// </summary>
        public PreviewDataPage()
        {
            InitializeComponent();
        }

        #region Obsługa zdarzeń generowanych przez użytkownika

        /// <summary>
        /// Obsługa przycisku "Wyślij".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonSend_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/UploadDataPage.xaml", UriKind.Relative));
        }

        #endregion Obsługa zdarzeń generowanych przez użytkownika
    }
}