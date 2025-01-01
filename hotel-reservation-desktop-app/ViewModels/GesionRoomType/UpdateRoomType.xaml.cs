
using System.Windows;


using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.ViewModels.GesionRoomType
{
    /// <summary>
    /// Logique d'interaction pour ModifyRooType.xaml
    /// </summary>
    public partial class UpdateRoomType : Window
    {
        private readonly GestionRoomTypesViewModel _viewModel;
        private readonly RoomType _roomTypeToEdit;

        // Constructeur qui reçoit un RoomType à modifier et le ViewModel
        public UpdateRoomType(GestionRoomTypesViewModel viewModel, RoomType roomType)
        {
            InitializeComponent();

            // Enregistrer le ViewModel et le RoomType à modifier
            _viewModel = viewModel;
            _roomTypeToEdit = roomType;

            // Lier l'objet à la fenêtre pour afficher les données
            DataContext = _roomTypeToEdit;
        }

        // Méthode pour modifier le RoomType
        private void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Appeler la méthode du ViewModel pour modifier le RoomType
                _viewModel.ModifierRoomType(_roomTypeToEdit);

                // Afficher un message de succès
                MessageBox.Show("Modification réussie!", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

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
    }
}
