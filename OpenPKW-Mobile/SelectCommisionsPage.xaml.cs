using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using OpenPKW_Mobile.Models;

namespace OpenPKW_Mobile
{
    /// <summary>
    /// Strona wyboru komisji
    /// </summary>
    /// <remarks>
    /// Wybrane komisje przekazywane są do App.CurrentAppData.SelectedCommisions
    /// </remarks>
    public partial class SelectCommisionsPage : PhoneApplicationPage
    {

        /// <summary>
        /// Pobranie danych - komisji
        /// </summary>
        /// <returns>Lista komisji przypisanych do użytkownika</returns>
        private IEnumerable<ElectoralCommision> GetData() {
            List<ElectoralCommision> commissions = new List<ElectoralCommision>();
            commissions.Add(new ElectoralCommision()
            {
                Id = 2,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 4,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 6,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 8,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 10,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 12,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 13,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommision()
            {
                Id = 14,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            return commissions;
        }

        /// <summary>
        /// Inicjalizacja
        /// </summary>
        public SelectCommisionsPage()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            listBoxCommissions.Items.Clear();
            listBoxCommissions.ItemsSource = GetData();
        }

        /// <summary>
        /// Obsługa przycisku Dalej
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentAppData.SelectedCommisions = listBoxCommissions.SelectedItems.Cast<ElectoralCommision>();
            MessageBox.Show("Wybrano pozycji: " + App.CurrentAppData.SelectedCommisions.Count());
        }
    }
}