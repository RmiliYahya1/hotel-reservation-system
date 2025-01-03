using System.Windows;
using System.Windows.Controls;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionChambre;

public partial class AjouterChambre : Window
{
    private readonly ChambreViewModel _viewModel;
    public AjouterChambre(ChambreViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        DataContext = _viewModel;

        // Remplir le ComboBox des catégories avec les types de chambre
        CategorieComboBox.ItemsSource = _viewModel.RoomTypes;
        CategorieComboBox.DisplayMemberPath = "Name";  
        CategorieComboBox.SelectedValuePath = "ID";  
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