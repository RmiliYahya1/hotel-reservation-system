using System.Windows;
using System.Windows.Input;
using hotel_reservation_desktop_app.View.gestionChambre;
using hotel_reservation_desktop_app.view.gestionClient;
using hotel_reservation_desktop_app.ViewModels;
using hotel_reservation_desktop_app.Views.GestionReservation;

namespace hotel_reservation_desktop_app.View.dashbord;

public partial class Dashbord 
{
    public Dashbord()
    {
        InitializeComponent();
        DataContext = new DashbordViewModel();
    }
    private void UIElement_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        if (mainWindow != null)
        {
         
            var gestionClientView = new MainClientSection();
            mainWindow.MainContentControl.Content = gestionClientView;
        }
    }

    private void ChambreRedirection(object sender, MouseButtonEventArgs e)
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        if (mainWindow != null)
        {
         
            var gestionChambreView = new ChambreMainSection();
            mainWindow.MainContentControl.Content = gestionChambreView;
        }
    }

    private void ReservationRedirection(object sender, MouseButtonEventArgs e)
    {
        MainWindow mainWindow = Application.Current.MainWindow as MainWindow;

        if (mainWindow != null)
        {
         
            var gestionReservationView = new Reservations();
            mainWindow.MainContentControl.Content = gestionReservationView;
        }
    }
}