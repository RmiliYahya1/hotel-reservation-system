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
    /// Logique d'interaction pour GestionRoomTypes.xaml
    /// </summary>
    public partial class GestionRoomTypes : UserControl
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
