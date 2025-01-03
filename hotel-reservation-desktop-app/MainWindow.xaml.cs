using System.Windows;
using hotel_reservation_desktop_app.View.Login;



namespace hotel_reservation_desktop_app;

public partial class MainWindow : Window
{


    public MainWindow()
    {
        InitializeComponent();

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
        UserControl1 userControl1 = new UserControl1();
        ContentControlMain.Content = userControl1;
    }

    private void ShowRoomControl(object sender, RoutedEventArgs e)
    {
        UserControl2 userControl2 = new UserControl2();
        ContentControlMain.Content = userControl2;
    }

}
