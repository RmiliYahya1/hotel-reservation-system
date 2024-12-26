using System.Windows;
using System.Windows.Controls;
using hotel_reservation_desktop_app.View.gestionClient;

namespace hotel_reservation_desktop_app.view.gestionClient;

public partial class MainClientSection : UserControl
{
    public MainClientSection()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        AjoutClient ajoutClient = new AjoutClient();
        ajoutClient.ShowDialog();
    }
}