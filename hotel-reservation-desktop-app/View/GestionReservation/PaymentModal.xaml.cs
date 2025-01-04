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
    /// Logique d'interaction pour PaymentModal.xaml
    /// </summary>
    public partial class PaymentModal : Window
    {
        public Payment Payment { get; }
        public PaymentModal()
        {
            InitializeComponent();
            
        }

        public void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
