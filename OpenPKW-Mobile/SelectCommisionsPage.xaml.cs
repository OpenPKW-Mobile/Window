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
    /// Stan zaznaczeń inicjalizowany jest listą App.CurrentAppData.SelectedCommisions.
    /// Wybrane przez użytkownika komisje przekazywane są do tej samej danej globalnej.
    /// </remarks>
    public partial class SelectCommisionsPage : PhoneApplicationPage
    {

        /// <summary>
        /// Pobranie danych - komisji
        /// </summary>
        /// <returns>Lista komisji przypisanych do użytkownika</returns>
        private IEnumerable<ElectoralCommission> getData() {
            List<ElectoralCommission> commissions = new List<ElectoralCommission>();
            //przykładowe dane
            commissions.Add(new ElectoralCommission()
            {
                Id = 2,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommission()
            {
                Id = 4,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            commissions.Add(new ElectoralCommission()
            {
                Id = 6,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommission()
            {
                Id = 8,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            commissions.Add(new ElectoralCommission()
            {
                Id = 10,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommission()
            {
                Id = 12,
                Title = "Żłobek nr 2",
                Address = "ul. Kantowa 32, 91-2222 Warszawa"
            });
            commissions.Add(new ElectoralCommission()
            {
                Id = 13,
                Title = "Szkoła Podstawowa nr 313 im. Bolka i Lolka",
                Address = "ul. Kantowa 32, 91-2222 Łódź"
            });
            commissions.Add(new ElectoralCommission()
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

        /// <summary>
        /// Obsługa zdarzenia załadowania strony
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            listBoxCommissions.Items.Clear();
            listBoxCommissions.ItemsSource = getData();
            initSelectedItems();

        }

        /// <summary>
        /// Inicjalizacja zaznaczeń listy komisji na podstawie Id komisji 
        /// w danych globalnych aplikacji App.CurrentAppData.SelectedCommisions
        /// </summary>
        /// <remarks>
        /// Metoda modyfikuje stan zaznaczeń na liście komisji
        /// </remarks>
        private void initSelectedItems()
        {
            listBoxCommissions.SelectedItems.Clear();
            if (App.CurrentAppData.SelectedCommisions != null)
            {
                var selectItemsQuery = from selectedCommision in App.CurrentAppData.SelectedCommisions
                                       join listItemCommision in listBoxCommissions.Items
                                       on selectedCommision.Id equals (listItemCommision as ElectoralCommission).Id
                                       select listItemCommision;
                foreach (var item in selectItemsQuery)
                {
                    listBoxCommissions.SelectedItems.Add(item);
                }
            }
        }

        /// <summary>
        /// Obsługa przycisku Dalej
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonNext_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentAppData.SelectedCommisions = listBoxCommissions.SelectedItems.Cast<ElectoralCommission>();
            MessageBox.Show("Wybrano pozycji: " + App.CurrentAppData.SelectedCommisions.Count());
        }
    }
}