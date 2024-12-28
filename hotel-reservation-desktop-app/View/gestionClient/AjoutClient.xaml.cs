using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionClient;

public partial class AjoutClient : Window
{
    public AjoutClient()
    {
        ClientViewModel clientViewModel = new ClientViewModel(this);
        DataContext = clientViewModel;
        InitializeComponent();
    }


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
    
}