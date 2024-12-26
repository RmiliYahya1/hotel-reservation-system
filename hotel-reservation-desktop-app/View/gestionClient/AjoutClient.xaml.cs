using System.Windows;
using System.Windows.Controls;
using hotel_reservation_desktop_app.ViewModels.gestionClient;

namespace hotel_reservation_desktop_app.View.gestionClient;

public partial class AjoutClient : Window
{
    public AjoutClient()
    {
        AjouterClientViewModel ajouterClientViewModel = new AjouterClientViewModel();
        InitializeComponent();
    }
    
    private void BtnClear_OnClick(object sender, RoutedEventArgs e)
    {
        NomInput.Clear();
        NomInput.Focus();
    }

    private void TxtInput_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(NomInput.Text))
        {
            NomPlaceholder.Visibility= Visibility.Visible;
        }
        else
        {
            NomPlaceholder.Visibility = Visibility.Hidden;
        }
    }
    
}