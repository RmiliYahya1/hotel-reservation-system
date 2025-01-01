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
    /// Logique d'interaction pour UpdateRoom.xaml
    /// </summary>
    public partial class UpdateRoom : Window
    {
        private readonly GestionRoomViewModel _viewModel;
        private readonly Room _roomToEdit;

        public UpdateRoom(GestionRoomViewModel viewModel, Room roomToEdit)
        {
            InitializeComponent();
            _viewModel = viewModel;
            _roomToEdit = roomToEdit;

            // Remplir le ComboBox des catégories avec les types de chambre
            CategorieComboBox.ItemsSource = _viewModel.RoomTypes;
            CategorieComboBox.DisplayMemberPath = "Name";  // Affiche le nom du type de chambre
            CategorieComboBox.SelectedValuePath = "ID";   // La valeur sélectionnée sera l'ID du RoomType

            // Pré-charger les données de la chambre dans les champs
            NumberTextBox.Text = _roomToEdit.Number;
            DisponibiliteComboBox.SelectedIndex = _roomToEdit.IsAvailable ? 0 : 1;  // "Disponible" ou "Indisponible"
            CategorieComboBox.SelectedValue = _roomToEdit.RoomTypeId;
        }

        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            var updatedRoom = new Room
            {
                ID = _roomToEdit.ID,
                Number = NumberTextBox.Text,
                IsAvailable = DisponibiliteComboBox.SelectedIndex == 0,
                RoomTypeId = (int)CategorieComboBox.SelectedValue
            };

            // Passer la chambre mise à jour au ViewModel pour la modification
            _viewModel.ModifierRoom(updatedRoom);
            this.DialogResult = true;
            this.Close();
        }
    }
}
