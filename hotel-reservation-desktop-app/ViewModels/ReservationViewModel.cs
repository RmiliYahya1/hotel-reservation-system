using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace hotel_reservation_desktop_app.ViewModels
{
    //code pour la vue model pour gérer les données des réservations
    public class ReservationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;


        public ObservableCollection<Reservation> Reservations { get; private set; }

        
        private Reservation _selectedReservation;
        public Reservation SelectedReservation
        {
            get { return _selectedReservation; }
            set { _selectedReservation = value; OnPropertyChanged("SelectedReservation"); }
        }
        public ObservableCollection<Client> clients { get; set; }

        private void UpdateAvailableRooms()
        {
            using var context = new HotelReservationContext();
            if (CheckInDate.HasValue && CheckOutDate.HasValue)
            {
                rooms = new ObservableCollection<Room>(
                    context.Rooms.ToList().Where(r =>
                        r.Reservations == null ||
                        r.Reservations.All(res => res.CheckInDate > CheckOutDate || res.CheckOutDate < CheckInDate))
                );
                OnPropertyChanged(nameof(rooms));
            }
        }
        public ObservableCollection<Room> rooms {
            get;
            set;
        }
        private Client _selectedClient;
        public Client SelectedClient
        {
            get { return _selectedClient; }
            set { _selectedClient = value; OnPropertyChanged("SelectedClient"); }
        }

        private Room _selectedRoom;
        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set { _selectedRoom = value; OnPropertyChanged("SelectedRoom"); }
        }
       
           
        private DateTime? _checkInDate;
        public DateTime? CheckInDate
        {
            get { return _checkInDate; }
            set { _checkInDate = value; UpdateAvailableRooms(); OnPropertyChanged(nameof(CheckInDate)); }
        }

        private DateTime? _checkOutDate;
        public DateTime? CheckOutDate
        {
            get { return _checkOutDate; }
            set { _checkOutDate = value; UpdateAvailableRooms(); OnPropertyChanged(nameof(CheckOutDate)); }
        }

        private double _price;
        public double Price
        {
            get { return _price; }
            set { _price = value; OnPropertyChanged("Price"); }
        }
        public ICommand SaveReservationCommand { get; }
        public ICommand CancelCommand { get; }

        public ReservationViewModel(Reservation reservation = null)
        {
            // charger données depuis la base de données
            using var context = new HotelReservationContext();
            clients = new ObservableCollection<Client>(context.Clients.ToList());
       
            // charger les reservations dans la liste des reservations
            Reservations = new ObservableCollection<Reservation>(context.Reservations.ToList());

            if (reservation != null)
            {
                SelectedClient = reservation.Client;
                SelectedRoom = reservation.Room;
                CheckInDate = reservation.CheckInDate;
                CheckOutDate = reservation.CheckOutDate;
                Price = reservation.Price;
            }

            SaveReservationCommand = new RelayCommand(SaveReservation);
            CancelCommand = new RelayCommand(Cancel);

            // charger les donnes de reservation dans le formulaire
            _selectedReservation = reservation;
            if (reservation != null)
            {
                SelectedClient = SelectedReservation.Client;
                SelectedRoom = SelectedReservation.Room;
                CheckInDate = SelectedReservation.CheckInDate;
                CheckOutDate = SelectedReservation.CheckOutDate;
                Price = SelectedReservation.Price;
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

}
