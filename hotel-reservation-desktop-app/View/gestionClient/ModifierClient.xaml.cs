using System.Windows;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionClient;

public partial class ModifierClient : Window
{
    private ClientViewModel _clientViewModel;
    private Client _clientToEdit;
    public ModifierClient(ClientViewModel clientViewModel, Client client)
    {
        InitializeComponent();
        _clientViewModel = clientViewModel;
        _clientToEdit = client;
        DataContext = _clientToEdit;
    }
    
    private void ModifierButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Appeler la méthode du ViewModel pour modifier le RoomType
            _clientViewModel.ModifierClient(_clientToEdit);
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