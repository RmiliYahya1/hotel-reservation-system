using System.Windows;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionUtilisateurs;

public partial class ModifierUser : Window
{
    private UserViewModel _userViewModel;
    private User _userToModify;
    public ModifierUser(UserViewModel userViewModel, User user)
    {
        InitializeComponent();
        _userViewModel = userViewModel;
        _userToModify=user;
        DataContext=_userToModify;
    }
    
    private void ModifierButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Appeler la méthode du ViewModel pour modifier le RoomType
            _userViewModel.ModifierUser(_userToModify);
            // Si la modification est réussie, fermer la fenêtre
            DialogResult = true;
            Close();
        }
        catch (Exception ex)
        {
            // En cas d'erreur, afficher un message
            MessageBox.Show($"Erreur : {ex.Message}");
        }
    }
    
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
}