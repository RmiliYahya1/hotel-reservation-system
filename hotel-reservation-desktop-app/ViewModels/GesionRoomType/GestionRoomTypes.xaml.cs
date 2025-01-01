using System.Windows;
using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.ViewModels.GesionRoomType
{
    public partial class GestionRoomTypes
    {
        public GestionRoomTypes()
        {
            InitializeComponent();
            this.DataContext = new GestionRoomTypesViewModel();
        }
        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddRoomType(DataContext as GestionRoomTypesViewModel);
            addWindow.ShowDialog();
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is RoomType roomTypeToEdit)
            {
                // Créer une instance de la fenêtre UpdateRoomType et passer le ViewModel et le RoomType à modifier
                var updateWindow = new UpdateRoomType(DataContext as GestionRoomTypesViewModel, roomTypeToEdit);
                var result = updateWindow.ShowDialog();

                // Si la modification a réussi, recharger les données
                if (result == true)
                {
                    (DataContext as GestionRoomTypesViewModel)?.LoadRoomTypes();
                }
            }
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is RoomType roomTypeToDelete)
            {
                var vm = DataContext as GestionRoomTypesViewModel;
                vm?.SupprimerRoomType(roomTypeToDelete);
            }
        }

       
    }
}
