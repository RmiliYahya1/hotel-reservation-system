using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace hotel_reservation_desktop_app.Views.GestionReservation
{
  
    public partial class Reservations : Window
    {
        public Reservations()
        {
            InitializeComponent();
            DataContext = new ReservationViewModel();       
        }
        // ouvrir le formulaire pour ajouter une réservation
        private void Ajouter_Button_Click(object sender, RoutedEventArgs e)
        {
            FormulaireReservation formulaireReservation = new();
            formulaireReservation.Show();
        }


        private void Supprimer_Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button != null)
            {
                var row = FindAncestor<DataGridRow>(button);
                if (row != null)
                {
                    Reservation? res = row.DataContext as Reservation;
                    if (res != null)
                    {
                        if (MessageBox.Show("Voulez-vous vraiment supprimer cette réservation ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                        {
                            if (DataContext is ReservationViewModel viewModel)
                            {
                                viewModel.DeleteReservation(res.ID);
                            }
                        }

                    }
                }
            }
        }
        private static T FindAncestor<T>(DependencyObject current) where T : DependencyObject
        {
            while (current != null && !(current is T))
            {
                current = VisualTreeHelper.GetParent(current);
            }
            return current as T;
        }

       
    }
}
