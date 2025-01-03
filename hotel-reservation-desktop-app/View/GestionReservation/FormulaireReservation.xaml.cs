using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using System.Windows;


namespace hotel_reservation_desktop_app.View.GestionReservation
{
    public partial class FormulaireReservation : Window
    {
        public Reservation Reservation { get; private set; }
        private ReservationViewModel _viewModel;
        public FormulaireReservation()
        {
            InitializeComponent();
            _viewModel = new ReservationViewModel();
            DataContext = _viewModel;
            


        }
        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            CloseWindow();
        }

        public void CloseWindow()
        {
            Window.GetWindow(this)?.Close();
        }

        private void Button_Click_add(object sender, RoutedEventArgs e)
        {
            _viewModel.SaveReservation();
            CloseWindow();
           

        }
    }

}
