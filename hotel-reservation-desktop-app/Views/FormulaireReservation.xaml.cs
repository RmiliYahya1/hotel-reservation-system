using hotel_reservation_DAL.Contexts;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hotel_reservation_desktop_app.Views
{
    /// <summary>
    /// Logique d'interaction pour FormulaireReservation.xaml
    /// </summary>
    public partial class FormulaireReservation : Window
    {
        public FormulaireReservation()
        {
            InitializeComponent();
            DataContext = new ReservationViewModel();

        }

    }


}
