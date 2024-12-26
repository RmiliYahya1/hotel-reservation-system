using System.Windows;
using System.Windows.Controls;
using hotel_reservation_desktop_app.View.gestionClient;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.view.gestionClient;

public partial class MainClientSection : UserControl
{
    public MainClientSection()
    {
        InitializeComponent();
        ClientViewModel clientViewModel = new ClientViewModel();
        DataContext = clientViewModel;
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        AjoutClient ajoutClient = new AjoutClient();
        ajoutClient.ShowDialog();
    }

    private void ClientSearch_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(clientSearch.Text))
        {
            TextBlock.Visibility= Visibility.Visible;
        }
        else
        {
            TextBlock.Visibility = Visibility.Hidden;
        }
    }
}