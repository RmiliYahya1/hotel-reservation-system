using System.Windows;
using hotel_reservation_desktop_app.view.gestionClient;
using hotel_reservation_desktop_app.View.dashbord;
using hotel_reservation_desktop_app.View.gestionChambre;
using hotel_reservation_desktop_app.View.GestionReservation;
using hotel_reservation_desktop_app.View.gestionRoomType;
using hotel_reservation_desktop_app.View.gestionUtilisateurs;
using hotel_reservation_desktop_app.View.Login;



namespace hotel_reservation_desktop_app;

public partial class MainWindow : Window
{


    public MainWindow()
    {
        InitializeComponent();
        ContentControlMain.Content = new View.dashbord.Dashbord();

    }
    private void CloseBtn_Click(object sender, RoutedEventArgs e)
    {
        Close();

    }

    private void MaxBtn_Click(object sender, RoutedEventArgs e)
    {
        if (WindowState == WindowState.Normal)
        {
            WindowState = WindowState.Maximized;
        }
        else
        {
            if (WindowState == WindowState.Maximized)
            {
                WindowState = WindowState.Normal;
            }
        }
    }

    private void ShowUserControl(object sender, RoutedEventArgs e)
    {
        
        MainClientSection mainClientSection = new MainClientSection();
        ContentControlMain.Content = mainClientSection;
    }


    private void ShowReservation(object sender, RoutedEventArgs e)
    {
        Reservations reservations = new Reservations();
        ContentControlMain.Content = reservations;
    }

    private void ShowRoomControl(object sender, RoutedEventArgs e)
    {
        UserControl2 userControl2 = new UserControl2();
        ContentControlMain.Content = userControl2;
    }

    private void ShowRoomType(object sender, RoutedEventArgs e)
    {
        RoomTypeMainSection roomType = new RoomTypeMainSection();
        ContentControlMain.Content = roomType;
    }

    private void ShowUser(object sender, RoutedEventArgs e)
    {
        MainUserSection userControl = new MainUserSection();
        ContentControlMain.Content = userControl;
    }

    private void ShowDashboard(object sender, RoutedEventArgs e)
    {
        Dashbord dashbord = new Dashbord();
        ContentControlMain.Content = dashbord;
    }

    private void ShowRoom(object sender, RoutedEventArgs e)
    {
        ChambreMainSection room = new ChambreMainSection();
        ContentControlMain.Content = room;
    }
}
