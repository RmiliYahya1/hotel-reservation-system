using System.Windows;
using System.Windows.Controls;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionChambre;

public partial class ChambreMainSection : UserControl
{
    public ChambreMainSection()
        {
            InitializeComponent();
            this.DataContext = new ChambreViewModel();
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AjouterChambre(DataContext as ChambreViewModel);
            addWindow.ShowDialog();
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as FrameworkElement)?.DataContext is Room roomToEdit)
            {
                // Créer une instance de la fenêtre UpdateRoom et passer le ViewModel et le Room à modifier
                var updateWindow = new ModifierChambre(DataContext as ChambreViewModel, roomToEdit);
                var result = updateWindow.ShowDialog();

                // Si la modification a réussi, recharger les données
                if (result == true)
                {
                    (DataContext as ChambreViewModel)?.LoadRooms();
                }
            }
        }

        private void SupprimerButton_Click(object sender, RoutedEventArgs e)
        {
            // Récupérer la chambre sélectionnée
            if ((sender as FrameworkElement)?.DataContext is Room roomToDelete)
            {
                var vm = DataContext as ChambreViewModel;
                vm?.SupprimerRoom(roomToDelete);
            }
        }

       
        private void PreviousButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ChambreViewModel;

            if (viewModel != null && viewModel.CurrentPage > 1)
            {
                viewModel.CurrentPage--;
                viewModel.LoadRooms();
            }
        }

       
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as ChambreViewModel;

            if (viewModel != null && viewModel.CurrentPage < viewModel.TotalPages)
            {
                viewModel.CurrentPage++;
                viewModel.LoadRooms();
            }
        }
}