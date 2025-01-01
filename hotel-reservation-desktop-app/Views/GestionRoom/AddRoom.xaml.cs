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

namespace hotel_reservation_desktop_app.Views.GestionRoom
{
    /// <summary>
    /// Logique d'interaction pour AddRoom.xaml
    /// </summary>
    public partial class AddRoom : Window
    {
        private readonly GestionRoomViewModel _viewModel;

        public AddRoom(GestionRoomViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            DataContext = _viewModel;

            // Remplir le ComboBox des catégories avec les types de chambre
            CategorieComboBox.ItemsSource = _viewModel.RoomTypes;
            CategorieComboBox.DisplayMemberPath = "Name";  // Affiche le nom du type de chambre
            CategorieComboBox.SelectedValuePath = "ID";   // La valeur sélectionnée sera l'ID du RoomType
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var newRoom = new Room
                {
                    Number = NumberTextBox.Text,
                    IsAvailable = DisponibiliteComboBox.SelectedIndex == 0, // Si "Disponible" est sélectionné
                    RoomTypeId = (int)CategorieComboBox.SelectedValue // Sélectionne l'ID du RoomType
                };

                // Appel de la méthode AjouterRoom dans le ViewModel
                _viewModel.AjouterRoom(newRoom);

                // Affichage du MessageBox de succès
                MessageBox.Show("La chambre a été ajoutée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                // Fermer la fenêtre après l'ajout
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                // Affichage du MessageBox d'erreur
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
