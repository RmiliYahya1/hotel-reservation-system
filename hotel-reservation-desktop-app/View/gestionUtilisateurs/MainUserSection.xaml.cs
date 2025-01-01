using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionUtilisateurs;

public partial class MainUserSection : UserControl
{
    private UserViewModel userViewModel;
    public MainUserSection()
    {
        InitializeComponent();
        userViewModel = new UserViewModel();
        DataContext = userViewModel;
    }
    
    private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null && !(current is T))
        {
            current = VisualTreeHelper.GetParent(current);
        }

        return current as T;
    }

    private void supprimer(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var row = FindAncestor<DataGridRow>(button);
            if (row != null)
            {
                User utilisateur = row.DataContext as User;
                if (utilisateur != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Êtes-vous sûr de vouloir supprimer ce client ?",
                        "Confirmation",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        userViewModel.RemoveUser(utilisateur.ID);
                    }
                }
            }
        }
    }

    private void Ajouter(object sender, RoutedEventArgs e)
    {
        var ajoutUtilisateur = new AjoutUtilisateur(userViewModel);
        ajoutUtilisateur.ShowDialog();
        userViewModel.LoadUsers(userViewModel.CurrentPage);
    }
    
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is User userTypeToEdit)
        {
            // Créer une instance de la fenêtre UpdateRoomType et passer le ViewModel et le RoomType à modifier
            var updateWindow = new ModifierUser(DataContext as UserViewModel, userTypeToEdit);
            var result = updateWindow.ShowDialog();

            // Si la modification a réussi, recharger les données
            if (result == true)
            {
                (DataContext as RoomTypeViewModel)?.LoadRoomTypes();
            }
        }
    }
}