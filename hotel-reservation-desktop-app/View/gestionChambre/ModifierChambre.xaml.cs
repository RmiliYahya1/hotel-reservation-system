using System.Windows;
using System.Windows.Controls;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionChambre;

public partial class ModifierChambre : Window
{
    private readonly ChambreViewModel _viewModel;
    private readonly Room _roomToEdit;
    public ModifierChambre(ChambreViewModel viewModel, Room roomToEdit)
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
            RoomTypeId = (int)CategorieComboBox.SelectedValue,
            RoomType = _viewModel.RoomTypes.FirstOrDefault(rt => rt.ID == (int)CategorieComboBox.SelectedValue), // Assurez-vous d'inclure RoomType
            Reservations = _roomToEdit.Reservations // Récupérez les réservations existantes
        };

        // Passer la chambre mise à jour au ViewModel pour la modification
        _viewModel.ModifierRoom(updatedRoom);
        this.DialogResult = true;
        this.Close();
    }
    
    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(NumberTextBox.Text))
        {
            TextBlock.Visibility= Visibility.Visible;
        }
        else
        {
            TextBlock.Visibility = Visibility.Hidden;
        }
    }
    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
}