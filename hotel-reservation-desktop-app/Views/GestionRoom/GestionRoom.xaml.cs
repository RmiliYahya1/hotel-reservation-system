using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using hotel_reservation_desktop_app.ViewModels.GesionRoomType;

namespace hotel_reservation_desktop_app.Views.GestionRoom
{
    /// <summary>
    /// Logique d'interaction pour GestionRoom.xaml
    /// </summary>
    public partial class GestionRoom : Window
    {
        public GestionRoom()
        {
            InitializeComponent();
            this.DataContext = new GestionRoomViewModel();
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddRoom(DataContext as GestionRoomViewModel);
            addWindow.ShowDialog();
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Room roomToEdit)
            {
                // Créer une instance de la fenêtre UpdateRoom et passer le ViewModel et le Room à modifier
                var updateWindow = new UpdateRoom(DataContext as GestionRoomViewModel, roomToEdit);
                var result = updateWindow.ShowDialog();

                // Si la modification a réussi, recharger les données
                if (result == true)
                {
                    (DataContext as GestionRoomViewModel)?.LoadRooms();
                }
            }
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la chambre sélectionnée
            if ((sender as FrameworkElement)?.DataContext is Room roomToDelete)
            {
                var vm = DataContext as GestionRoomViewModel;
                vm?.SupprimerRoom(roomToDelete);
            }
        }

        // Gestionnaire pour le bouton Précédent
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as GestionRoomViewModel;

            if (viewModel != null && viewModel.CurrentPage > 1)
            {
                viewModel.CurrentPage--;
                viewModel.LoadRooms();
            }
        }

        // Gestionnaire pour le bouton Suivant
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as GestionRoomViewModel;

            if (viewModel != null && viewModel.CurrentPage < viewModel.TotalPages)
            {
                viewModel.CurrentPage++;
                viewModel.LoadRooms();
            }
        }

    }
}
