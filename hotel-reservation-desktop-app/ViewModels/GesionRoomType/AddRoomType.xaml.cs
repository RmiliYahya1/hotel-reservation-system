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
using hotel_reservation_desktop_app.ViewModels.GesionRoomType;

namespace hotel_reservation_desktop_app.ViewModels
{
    /// <summary>
    /// Logique d'interaction pour AddUpdateRoomType.xaml
    /// </summary>
    public partial class AddRoomType : Window
    {
        private readonly GestionRoomTypesViewModel _viewModel;

        public AddRoomType(GestionRoomTypesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
        }

        private void AjouterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Récupérer les données des TextBox
                string name = NomTxt.Text;
                string description = DescriptionTxt.Text;
                double price = double.Parse(PrixTxt.Text);
                int capacity = int.Parse(CapaciteTxt.Text);

                // Créer un nouvel objet RoomType
                var newRoomType = new RoomType
                {
                    Name = name,
                    Description = description,
                    Price = price,
                    Capacity = capacity
                };

                // Ajouter via le ViewModel
                _viewModel.AjouterRoomType(newRoomType);

                MessageBox.Show("RoomType ajouté avec succès !");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur : {ex.Message}");
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this)?.Close();
        }
    }
}
