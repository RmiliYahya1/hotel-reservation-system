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

namespace hotel_reservation_desktop_app.Views.GestionReservation
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
        // ouvrir le formulaire pour ajouter une réservation
        private void Ajouter_Button_Click(object sender, RoutedEventArgs e)
        {
            FormulaireReservation formulaireReservation = new FormulaireReservation();
            formulaireReservation.ShowDialog();
        }

        // // ouvrir le formulaire pour modifier une réservation
        private void Modifier_Button_Click(object sender, RoutedEventArgs e)
        {             


        }
    }
}
