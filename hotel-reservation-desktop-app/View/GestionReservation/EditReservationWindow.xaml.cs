using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using System.Windows;


namespace hotel_reservation_desktop_app.View.GestionReservation
{
    /// <summary>
    /// Logique d'interaction pour EditReservationWindow.xaml
    /// </summary>
    public partial class EditReservationWindow : Window
    {
        public Reservation EditedReservation { get; private set; }
        public EditReservationWindow(Reservation reservation)
        {
            InitializeComponent();
            using var context = new HotelReservationContext();
            // Clear existing items
            ClientComboBox.Items.Clear();
            RoomComboBox.Items.Clear();
            RoomTypeComboBox.Items.Clear();

            // Charger les listes de clients et chambres
            ClientComboBox.ItemsSource = context.Clients.ToList();
            RoomComboBox.ItemsSource = context.Rooms.ToList();
            RoomTypeComboBox.ItemsSource = context.RoomTypes.ToList();

            // Pré-remplir les champs avec les données de la réservation
            EditedReservation = reservation;

            ClientComboBox.SelectedValue = reservation.ClientId;
            RoomComboBox.SelectedValue = reservation.RoomId;
            RoomTypeComboBox.SelectedValue = reservation.Room.RoomTypeId;
            CheckInDatePicker.SelectedDate = reservation.CheckInDate;
            CheckOutDatePicker.SelectedDate = reservation.CheckOutDate;
            DataContext = new ReservationViewModel();
        }


        private void DatePicker_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            // Vérifier si les deux dates sont sélectionnées
            if (CheckInDatePicker.SelectedDate != null && CheckOutDatePicker.SelectedDate != null)
            {
                var checkIn = CheckInDatePicker.SelectedDate.Value;
                var checkOut = CheckOutDatePicker.SelectedDate.Value;
                var roomTypeId = (int)RoomTypeComboBox.SelectedValue;
                // Appeler la méthode pour charger les chambres disponibles
                LoadAvailableRooms(checkIn, checkOut, roomTypeId);
            }

        }


        private void LoadAvailableRooms(DateTime checkIn, DateTime checkOut, int roomTypeId)
        {
            using var context = new HotelReservationContext();
            // Récupérer les chambres qui ne sont pas réservées dans l'intervalle de dates et du type sélectionné
            var availableRooms = context.Rooms
                .Where(room => !context.Reservations
                    .Any(reservation =>
                        reservation.RoomId == room.ID &&
                        ((reservation.CheckInDate <= checkOut && reservation.CheckOutDate >= checkIn))))
                .Where(room => room.RoomTypeId == roomTypeId)
                .ToList();
            // Mettre à jour le ComboBox avec les chambres disponibles
            RoomComboBox.ItemsSource = availableRooms;
            RoomComboBox.SelectedValue = EditedReservation.RoomId; // Pré-sélectionner la chambre existante si toujours disponible
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientComboBox.SelectedValue == null || RoomComboBox.SelectedValue == null ||
                CheckInDatePicker.SelectedDate == null || CheckOutDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Veuillez remplir tous les champs.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Mettre à jour l'objet réservation
            EditedReservation.ClientId = (int)ClientComboBox.SelectedValue;
            EditedReservation.RoomId = (int)RoomComboBox.SelectedValue;
            EditedReservation.CheckInDate = CheckInDatePicker.SelectedDate.Value;
            EditedReservation.CheckOutDate = CheckOutDatePicker.SelectedDate.Value;
            EditedReservation.Price = (EditedReservation.CheckOutDate - EditedReservation.CheckInDate).Days * EditedReservation.Room.RoomType.Price;

            DialogResult = true; // Indiquer que l'édition a été validée
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close(); // Fermer la fenêtre sans valider
        }

        private void RoomTypeComboBox_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            // charger les chambres disponibles si le type de chambre est changé
            if (CheckInDatePicker.SelectedDate != null && CheckOutDatePicker.SelectedDate != null)
            {
                var checkIn = CheckInDatePicker.SelectedDate.Value;
                var checkOut = CheckOutDatePicker.SelectedDate.Value;
                var roomTypeId = (int)RoomTypeComboBox.SelectedValue;
                // Appeler la méthode pour charger les chambres disponibles
                LoadAvailableRooms(checkIn, checkOut, roomTypeId);
            }



        }
    }
}
