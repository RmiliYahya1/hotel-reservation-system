using System.Configuration;
using System.Data;
using System.Windows;
using hotel_reservation_desktop_app.View.Login;

namespace hotel_reservation_desktop_app;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected void ApplicationStart(object sender, StartupEventArgs e)
    {
        var loginView = new LoginView();
        loginView.Show();
        loginView.IsVisibleChanged += (s, ev) =>
        {
            if (loginView.IsVisible == false && loginView.IsLoaded)
            {
                var mainView = new MainWindow();
                mainView.Show();
                loginView.Close();

            }
        };

    }
}