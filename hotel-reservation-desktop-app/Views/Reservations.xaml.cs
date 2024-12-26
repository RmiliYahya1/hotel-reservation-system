using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace hotel_reservation_desktop_app.Views
{
    /// <summary>
    /// Logique d'interaction pour Reservations.xaml
    /// </summary>
    public partial class Reservations : Window
    {
        public Reservations()
        {
            InitializeComponent();
            DataContext = new ReservationViewModel();
        }

        private void Ajouter_Button_Click(object sender, RoutedEventArgs e)
        {
            // ouvrir le user control pour ajouter une réservation
            FormulaireReservation formulaireReservation = new FormulaireReservation();
            formulaireReservation.ShowDialog();
        }

        // modifier une réservation
        private void Modifier_Button_Click(object sender, RoutedEventArgs e)
        {             


        }

        // charger les réservations dans le datagrid
        private void DataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            using var context = new HotelReservationContext();
            var reservations = context.Reservations.ToList();
            var dataGrid = sender as DataGrid;
            ReservationDataGrid = dataGrid;
            dataGrid.ItemsSource = reservations;
        }

        private void ReservationDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // recuperer la reservation selectionne
            var reservation = ReservationDataGrid.SelectedItem as Reservation;
            if (reservation != null)
            {
                // afficher les informations de la reservation selectionne dans le formulaire
                var viewModel = DataContext as ReservationViewModel;
                viewModel.SelectedClient = reservation.Client;
                viewModel.SelectedRoom = reservation.Room;
                viewModel.CheckInDate = reservation.CheckInDate;
                viewModel.CheckOutDate = reservation.CheckOutDate;
                viewModel.Price = reservation.Price;

            }
        }
    }
}
