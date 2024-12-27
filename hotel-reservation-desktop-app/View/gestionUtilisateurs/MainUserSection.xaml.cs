using System.Windows;
using System.Windows.Controls;

namespace hotel_reservation_desktop_app.View.gestionUtilisateurs;

public partial class MainUserSection : UserControl
{
    public MainUserSection()
    {
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
       AjoutUtilisateur ajoutUtilisateur = new AjoutUtilisateur();
       ajoutUtilisateur.ShowDialog();
    }

    private void ClientSearch_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(usercherche.Text))
        {
            TextBlock.Visibility= Visibility.Visible;
        }
        else
        {
            TextBlock.Visibility = Visibility.Hidden;
        }
    }
}