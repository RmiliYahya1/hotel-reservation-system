using System.Windows;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionUtilisateurs;

public partial class AjoutUtilisateur : Window
{
    public AjoutUtilisateur(UserViewModel userViewModel)
    {
        userViewModel = new UserViewModel(this);
        DataContext = userViewModel;
        InitializeComponent();
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
}