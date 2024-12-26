using System.Windows;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionClient;

public partial class AjoutClient : Window
{
    public AjoutClient()
    {
        InitializeComponent();
        ClientViewModel clientViewModel = new ClientViewModel();
        DataContext = clientViewModel;
    }


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
}