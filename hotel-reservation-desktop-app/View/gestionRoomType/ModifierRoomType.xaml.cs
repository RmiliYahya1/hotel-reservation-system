using System.Windows;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionRoomType;

public partial class ModifierRoomType : Window
{
    
    private RoomTypeViewModel _roomTypeViewModel;
    private RoomType _roomTypeToEdit;
    public ModifierRoomType(RoomTypeViewModel? viewModel, RoomType roomType)
    {
        InitializeComponent();
        _roomTypeViewModel = viewModel;
        _roomTypeToEdit = roomType;
        DataContext = _roomTypeToEdit;
    }
    
    private void ModifierButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Appeler la méthode du ViewModel pour modifier le RoomType
            _roomTypeViewModel.ModifierRoomType(_roomTypeToEdit);
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