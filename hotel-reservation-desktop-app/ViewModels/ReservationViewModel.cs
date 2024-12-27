/*using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public ObservableCollection<Reservation> Reservations { get; private set; }
        
      private Reservation _selectedReservation;
      public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set { _selectedReservation = value; OnPropertyChanged(nameof(SelectedReservation)); }
        }
        public ObservableCollection<Client> Clients { get; set; }

        public ObservableCollection<Room> Rooms {
            get;
            set;
        }
        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged(nameof(SelectedClient)); }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set { _selectedRoom = value; OnPropertyChanged(nameof(SelectedRoom));
                UpdatePrice();
            }
        }

       

        private  DateTime? _checkInDate;
        public DateTime? CheckInDate
        {
            get { return _checkInDate; }
            set { _checkInDate = value; 
                OnPropertyChanged(nameof(CheckInDate));
                UpdateAvailableRoomsAndPrice();
            }
        }

       

        private DateTime? _checkOutDate;
        public DateTime? CheckOutDate
        {
            get { return _checkOutDate; }
            set { _checkOutDate = value; 
                OnPropertyChanged(nameof(CheckOutDate));
                UpdateAvailableRoomsAndPrice();
            }
        }


        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; 
                OnPropertyChanged(nameof(Price));
                UpdateAvailableRoomsAndPrice();
            }
        }
        public ICommand SaveReservationCommand { get; }
        public ICommand CancelCommand { get; }

        public ReservationViewModel(Reservation reservation = null)
        {
            // charger les clients et rooms depuis la base de données
            using var context = new HotelReservationContext();
            Clients = new ObservableCollection<Client>([.. context.Clients]);
            Rooms = new ObservableCollection<Room>([.. context.Rooms]);

            // charger les reservations dans la liste des reservations
            Reservations = new ObservableCollection<Reservation>([.. context.Reservations]);

            SaveReservationCommand = new RelayCommand(SaveReservation);
            CancelCommand = new RelayCommand(Cancel);

        }

        private void UpdateAvailableRoomsAndPrice()
        {
            using var context = new HotelReservationContext();
            var availableRooms = context.Rooms
               .Where(r => r.Reservations.All(res => res.CheckInDate > CheckOutDate || res.CheckOutDate < CheckInDate))
               .ToList();
            Rooms = new ObservableCollection<Room>(availableRooms);
            OnPropertyChanged(nameof(Rooms));
            UpdatePrice();
        }

        private void UpdatePrice()
        {
            if (SelectedRoom != null && SelectedRoom.RoomType != null && CheckInDate.HasValue && CheckOutDate.HasValue)
            {
                // calculer le nombre de jours entre CheckInDate et CheckOutDate
                var days = (CheckOutDate.Value - CheckInDate.Value).Days;
                Price = days * SelectedRoom.RoomType.Price;

            }
        }

        private void SaveReservation()
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
        }

        private void Cancel()
        {
            SelectedClient = null;
            SelectedRoom = null;
            CheckInDate = null;
            CheckOutDate = null;
            Price = 0;
        }

        





        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}*/

using GalaSoft.MvvmLight.Command;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Client> Clients { get; private set; }
        public ObservableCollection<Room> Rooms { get; private set; }
        public ObservableCollection<Room> FilteredRooms { get; private set; }
        public ObservableCollection<Reservation> Reservations { get; private set; }

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
            set { _price = value; OnPropertyChanged(nameof(Price)); }
        }

        public ICommand SaveReservationCommand { get; }
        public ICommand CancelCommand { get; }

        public ReservationViewModel()
        {
            Clients = new ObservableCollection<Client>();
            Rooms = new ObservableCollection<Room>();
            FilteredRooms = new ObservableCollection<Room>();
            Reservations = new ObservableCollection<Reservation>();

            SaveReservationCommand = new RelayCommand(SaveReservation, CanSaveReservation);
            CancelCommand = new RelayCommand(Cancel);

            LoadData();
        }

        private void LoadData()
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
            if (!CheckInDate.HasValue || !CheckOutDate.HasValue)
            {
                FilteredRooms = new ObservableCollection<Room>(Rooms);
                OnPropertyChanged(nameof(FilteredRooms));
                return;
            }

            var unavailableRoomIds = Reservations
                .Where(r =>
                    (CheckInDate <= r.CheckOutDate && CheckOutDate >= r.CheckInDate))
                .Select(r => r.RoomId);

            FilteredRooms = new ObservableCollection<Room>(
                Rooms.Where(r => !unavailableRoomIds.Contains(r.ID))
            );

            OnPropertyChanged(nameof(FilteredRooms));
        }

        private void UpdatePrice()
        {
            if (SelectedRoom != null && SelectedRoom.RoomType != null && CheckInDate.HasValue && CheckOutDate.HasValue)
            {
                var days = (CheckOutDate.Value - CheckInDate.Value).Days;
                Price = days * SelectedRoom.RoomType.Price;
                if (Price < 0)
                {
                    Price = 0;
                }
            }
            else
            {
                Price = 0;
            }
        }

        public bool CanSaveReservation()
        {
            return SelectedClient != null && SelectedRoom != null &&
                   CheckInDate.HasValue && CheckOutDate.HasValue &&
                   CheckInDate < CheckOutDate;
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
    }
}

