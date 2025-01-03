using hotel_reservation_DAL.Entities;
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

namespace hotel_reservation_desktop_app.View.GestionReservation
{
    /// <summary>
    /// Logique d'interaction pour FormulairePaiement.xaml
    /// </summary>
    public partial class FormulairePaiement : Window
    {
        public Payment NewPayment { get; private set; }

        public FormulairePaiement()
        {
            InitializeComponent();
        }

        public FormulairePaiement(Reservation reservation)
        {
            InitializeComponent();

            // Afficher le prix de la réservation comme montant du paiement
            AmountTextBlock.Text = reservation.Price.ToString("F2"); // Format avec 2 décimales
        }

        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            // Validation des données entrées
            if (PaymentMethodComboBox.SelectedItem is ComboBoxItem selectedMethod)
            {
                // Créer un nouvel objet Payment avec la date actuelle
                NewPayment = new Payment
                {
                    Amount = (double)decimal.Parse(AmountTextBlock.Text),
                    Date = DateTime.Now, // Date et heure actuelles
                    PaymentMethod = selectedMethod.Content.ToString()
                };

                DialogResult = true; // Confirmer l'ajout
                this.Close();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner une méthode de paiement.",
                                 "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Fermer sans rien faire
        }
    }
}
