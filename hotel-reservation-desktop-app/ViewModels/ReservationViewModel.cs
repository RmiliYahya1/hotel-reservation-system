using GalaSoft.MvvmLight.Command;

using hotel_reservation_desktop_app.Views.GestionReservation;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {

        public HotelReservationContext _context;
        public event PropertyChangedEventHandler? PropertyChanged;
      


        public ObservableCollection<Client> Clients { get; private set; }
        public ObservableCollection<Room> Rooms { get; private set; }
       
        public ObservableCollection<Room> FilteredRooms { get; set; } = [];
        public ObservableCollection<Reservation> Reservations { get; private set; }
        public ObservableCollection<RoomType> RoomTypes { get; set; }
        private RoomType _selectedRoomType;
        public RoomType SelectedRoomType
        {
            get => _selectedRoomType;
            set
            {
                _selectedRoomType = value;
                OnPropertyChanged(nameof(SelectedRoomType));
                UpdateFilteredRooms(); // Mettre à jour les chambres disponibles en fonction du type
                
                //UpdatePrice();
                
            }
        }

        private Reservation _selectedReservation;
        public Reservation SelectedReservation
        {
            get => _selectedReservation;
            set
            {
                _selectedReservation = value;
                OnPropertyChanged(nameof(SelectedReservation));
                
            }


        }


        private Client _selectedClient;
        public Client SelectedClient
        {
            get => _selectedClient;
            set { _selectedClient = value; 
                OnPropertyChanged(nameof(SelectedClient));
                ((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
            }
        }

        private Room _selectedRoom;
        
        public Room SelectedRoom
        {
            get => _selectedRoom;
            set { _selectedRoom = value; 
                OnPropertyChanged(nameof(SelectedRoom)); 
                UpdatePrice();
               

                ((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
            }
        }

        private DateTime? _checkInDate;
        public DateTime? CheckInDate
        {
            get => _checkInDate;
            set
            {
                _checkInDate = value;
                OnPropertyChanged(nameof(CheckInDate));
                UpdateFilteredRooms();
                UpdatePrice();
               

                ((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
            }
        }

        private DateTime? _checkOutDate;
        public DateTime? CheckOutDate
        {
            get => _checkOutDate;
            set
            {
                _checkOutDate = value;
                OnPropertyChanged(nameof(CheckOutDate));
                UpdateFilteredRooms();
                UpdatePrice();
               

                ((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set {
               
                    _price = value;
                    OnPropertyChanged(nameof(Price));
                

            }
        }

        private string _status;
        public string Status
        {
            get => _status;
            set { _status = value; OnPropertyChanged(nameof(Status)); }

        }

        public ICommand SaveReservationCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand DeleteReservationCommande { get; }
        public ICommand ViewPaymentCommand { get; }
        public RelayCommand<Reservation> AddPaymentCommand { get; set; }
        public RelayCommand<Reservation> EditReservationCommand { get; set; }


        public ReservationViewModel()
        {
            _context = new HotelReservationContext();
            Clients = [];
            Rooms = [];
            FilteredRooms = [];
            Reservations = [];

            SaveReservationCommand = new RelayCommand(SaveReservation, CanSaveReservation);
            CancelCommand = new RelayCommand(Cancel);
            LoadData();
            ViewPaymentCommand = new RelayCommand<Reservation>(ViewPayment);
            AddPaymentCommand = new RelayCommand<Reservation>(AddPayment);

            EditReservationCommand = new RelayCommand<Reservation>(OpenEditReservationWindow);
            LoadRoomTypes();


        }

        // Méthode pour charger les types de chambres
        public void LoadRoomTypes()
        {
            using var context = new HotelReservationContext();
            RoomTypes = new ObservableCollection<RoomType>([.. context.RoomTypes]);
        }

        public static void ViewPayment(Reservation reservation)
        {
            using var context = new HotelReservationContext();
            var res = context.Reservations
                .Include(r => r.Payment)
                .Include(r => r.Client)
                .FirstOrDefault(r => r.ID == reservation.ID);

            // Vérifier si la réservation existe
            if (res != null)
            {
                // Vérifier si un paiement est associé
                if (res.Payment != null)
                {
                    // Créer et afficher la fenêtre modale avec les informations de paiement
                    var paymentWindow = new PaymentModal
                    {
                        DataContext = res // Lier l'objet réservation (incluant le paiement) à la fenêtre
                    };
                    paymentWindow.Show(); // Afficher en modal
                }
                else
                {
                    MessageBox.Show("Aucun paiement associé à cette réservation.",
                                     "Information",
                                     MessageBoxButton.OK,
                                     MessageBoxImage.Warning);
                }
            }
            else
            {
                MessageBox.Show("Réservation introuvable.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private void AddPayment(Reservation reservation)
        {
            if (reservation == null)
            {
                MessageBox.Show("Veuillez sélectionner une réservation.",
                                "Information", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var addPaymentWindow = new FormulairePaiement(reservation);
            var result = addPaymentWindow.ShowDialog(); // Modal bloquant

            if (result == true && addPaymentWindow.NewPayment != null)
            {
                // Ajouter le paiement à la réservation
                using var context = new HotelReservationContext();
                var res = context.Reservations.Include(r => r.Payment).FirstOrDefault(r => r.ID == reservation.ID);

                if (res != null)
                {
                    res.Payment = addPaymentWindow.NewPayment; // Associer le paiement
                    context.SaveChanges(); // Sauvegarder les changements

                    MessageBox.Show("Le paiement a été ajouté avec succès.",
                                    "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour de la réservation.",
                                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadData();
                OnPropertyChanged(nameof(Reservations));
            }
        }

        public void DeleteReservation(int reservationId)
        {

            var reservationToRemove = _context.Reservations.FirstOrDefault(c => c.ID == reservationId);
            if (reservationToRemove != null)
            {
                _context.Reservations.Remove(reservationToRemove);
                _context.SaveChanges();
                LoadData();
                OnPropertyChanged(nameof(Reservations));
            }

        }


        public void LoadData()
        {
            using var context = new HotelReservationContext();

            // Charger les données depuis la base
            var clients = context.Clients.ToList();
            var rooms = context.Rooms.Include(r => r.RoomType).ToList();
            var reservations = context.Reservations.ToList();

            Clients = new ObservableCollection<Client>(clients);
            Rooms = new ObservableCollection<Room>(rooms);
            FilteredRooms = new ObservableCollection<Room>(rooms); // Filtrée dynamiquement
            Reservations = new ObservableCollection<Reservation>(reservations);
        }

        private void UpdateFilteredRooms()
        {
            if (SelectedRoomType == null)
            {
                FilteredRooms.Clear();
                return;
            }

            using var context = new HotelReservationContext();

            // Récupérer les chambres correspondant au type sélectionné et aux dates
            var availableRooms = context.Rooms
                .Where(r => r.RoomTypeId == SelectedRoomType.ID &&
                            !context.Reservations.Any(res =>
                                res.RoomId == r.ID &&
                                ((res.CheckInDate <= CheckOutDate && res.CheckOutDate >= CheckInDate))))
                .ToList();

            FilteredRooms.Clear();
            foreach (var room in availableRooms)
            {
                FilteredRooms.Add(room);
            }
            if (!FilteredRooms.Any())
            {
                MessageBox.Show("Aucune chambre disponible pour ces dates.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }



        private void UpdatePrice()
        {
            //Console.WriteLine($"SelectedRoom: {SelectedRoom?.Number}, SelectedRoomType: {SelectedRoom?.RoomType.Name}, CheckInDate: {CheckInDate}, CheckOutDate: {CheckOutDate}");

            if (SelectedRoom != null && CheckInDate.HasValue && CheckOutDate.HasValue)
            {
                var days = (CheckOutDate.Value - CheckInDate.Value).Days;
                if (days < 1) // Si les dates sont incorrectes (CheckOut avant CheckIn)
                {
                    Price = 0;
                }
                else
                {
                    Price = days * SelectedRoomType.Price;
                }
            }
          

            else
            {
                Price = 0;
            }
            

            Console.WriteLine($"Price: {Price}");
        }



        public bool CanSaveReservation()
        {
            return SelectedClient != null && SelectedRoom != null &&
                SelectedRoomType != null &&
                   CheckInDate.HasValue && CheckOutDate.HasValue &&
                   CheckInDate < CheckOutDate && Price > 0;
        }

        private void SaveReservation()
        {
            var result = MessageBox.Show("Voulez-vous vraiment enregistrer cette réservation ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                using var context = new HotelReservationContext();
      
                    Reservation reservation = new()
                    {
                        ClientId = SelectedClient.ID,
                        RoomId = SelectedRoom.ID,
                        CheckInDate = CheckInDate ?? DateTime.Now,
                        CheckOutDate = CheckOutDate ?? DateTime.Now,
                        Date = DateTime.Now,
                        Price = Price
                    };

                    context.Reservations.Add(reservation);
                    context.SaveChanges();
                    LoadData();
                    OnPropertyChanged(nameof(Reservations));
                    MessageBox.Show("Réservation enregistrée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                    
            }
        }
  
        private void Cancel()
        {
            var result = MessageBox.Show("Voulez-vous vraiment annuler cette opération ?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                SelectedClient = null;
                SelectedRoom = null;
                CheckInDate = null;
                CheckOutDate = null;
                Price = 0;

                MessageBox.Show("Opération annulée.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void OpenEditReservationWindow(Reservation reservation)
        {
            if (reservation == null)
            {
                MessageBox.Show("Veuillez sélectionner une réservation.",
                                "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ouvrir le modal pour modifier la réservation
            var editReservationWindow = new EditReservationWindow(reservation);
            var result = editReservationWindow.ShowDialog();

            if (result == true)
            {
                // Enregistrer les modifications en base de données
                using var context = new HotelReservationContext();
                var reservationToUpdate = context.Reservations.FirstOrDefault(r => r.ID == reservation.ID);

                if (reservationToUpdate != null)
                {
                    reservationToUpdate.ClientId = reservation.ClientId;
                    reservationToUpdate.RoomId = reservation.RoomId;
                    reservationToUpdate.CheckInDate = reservation.CheckInDate;
                    reservationToUpdate.CheckOutDate = reservation.CheckOutDate;
                    reservationToUpdate.Price =(reservation.CheckOutDate - reservation.CheckInDate).Days * reservation.Room.RoomType.Price;

                    context.SaveChanges();

                    MessageBox.Show("Réservation modifiée avec succès.",
                                    "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erreur lors de la mise à jour de la réservation.",
                                    "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                // Rafraîchir la liste des réservations
                LoadData();
                OnPropertyChanged(nameof(Reservations));
            }
        }




    }
}

