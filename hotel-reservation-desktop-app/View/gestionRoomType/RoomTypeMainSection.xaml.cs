using System.Windows;
using System.Windows.Controls;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;


namespace hotel_reservation_desktop_app.View.gestionRoomType;

public partial class RoomTypeMainSection : UserControl
{
    public RoomTypeMainSection()
    {
        InitializeComponent();
        DataContext = new RoomTypeViewModel();
    }
    
    private void AjouterButton_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AjouterRoomType(DataContext as RoomTypeViewModel);
        addWindow.ShowDialog();
    }

    private void ModifierButton_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is RoomType roomTypeToEdit)
        {
            // Créer une instance de la fenêtre UpdateRoomType et passer le ViewModel et le RoomType à modifier
            var updateWindow = new ModifierRoomType(DataContext as RoomTypeViewModel, roomTypeToEdit);
            var result = updateWindow.ShowDialog();

            // Si la modification a réussi, recharger les données
            if (result == true)
            {
                (DataContext as RoomTypeViewModel)?.LoadRoomTypes();
            }
        }
    }

    private void SupprimerButton_Click(object sender, RoutedEventArgs e)
    {
        if ((sender as FrameworkElement)?.DataContext is RoomType roomTypeToDelete)
        {
            var vm = DataContext as RoomTypeViewModel;
            vm?.SupprimerRoomType(roomTypeToDelete);
        }
    }
}