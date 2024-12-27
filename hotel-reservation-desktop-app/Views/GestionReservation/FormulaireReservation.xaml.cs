using hotel_reservation_DAL.Contexts;
using hotel_reservation_desktop_app.ViewModels;
using System.Windows;


namespace hotel_reservation_desktop_app.Views.GestionReservation
{
    public partial class FormulaireReservation : Window
    {
        public FormulaireReservation()
        {
            InitializeComponent();
            DataContext = new ReservationViewModel();

        }

    }


}
