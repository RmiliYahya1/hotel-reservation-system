using GalaSoft.MvvmLight.Command;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.View.GestionReservation;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using OfficeOpenXml;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using System.IO;
using System.Net.Mail;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class ReservationViewModel : INotifyPropertyChanged
    {

        private string _filterText;
        public string FilterText
        {
            get => _filterText;
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                FilterReservations();
            }
        }

        private ObservableCollection<Reservation> _filteredReservations;
        public ObservableCollection<Reservation> FilteredReservations
        {
            get => _filteredReservations;
            set
            {
                _filteredReservations = value;
                OnPropertyChanged(nameof(FilteredReservations));
            }
        }

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
            set
            {
                _selectedClient = value;
                OnPropertyChanged(nameof(SelectedClient));
                //((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
            }
        }

        private Room _selectedRoom;

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                OnPropertyChanged(nameof(SelectedRoom));
                UpdatePrice();


                //((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
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


                //((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
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


                //((RelayCommand)SaveReservationCommand).RaiseCanExecuteChanged();
            }
        }

        private double _price;
        public double Price
        {
            get => _price;
            set
            {

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
        public ICommand CancelCommand { get; }
        public ICommand DeleteReservationCommande { get; }
        public RelayCommand ExportReservationsToExcelCommand { get; }


        public ReservationViewModel()
        {
            _context = new HotelReservationContext();
            Clients = [];
            Rooms = [];
            FilteredRooms = [];
            Reservations = [];
            LoadData();
            LoadRoomTypes();
            ExportReservationsToExcelCommand = new RelayCommand(ExportReservations, CanExportReservations);
            FilteredReservations = new ObservableCollection<Reservation>(Reservations);

        }

        private void FilterReservations()
        {
            if (string.IsNullOrWhiteSpace(FilterText))
            {
                FilteredReservations = new ObservableCollection<Reservation>(Reservations);
            }
            else
            {
                FilteredReservations = new ObservableCollection<Reservation>(
                    Reservations.Where(r => r.Client.FullName.IndexOf(FilterText, StringComparison.OrdinalIgnoreCase) >= 0));
            }
        }

        // Méthode pour charger les types de chambres
        public void LoadRoomTypes()
        {
            using var context = new HotelReservationContext();
            RoomTypes = new ObservableCollection<RoomType>([.. context.RoomTypes]);
        }

        public void ViewPayment(Reservation reservation)
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
                    paymentWindow.ShowDialog(); // Afficher en modal
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



        public void AddPayment(Reservation reservation)
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
                var res = context.Reservations
                    .Include(r => r.Payment)
                    .Include(r => r.Client)
                    .Include(r => r.Room)
                   .FirstOrDefault(r => r.ID == reservation.ID);


                if (res != null)
                {
                    res.Payment = addPaymentWindow.NewPayment; // Associer le paiement
                    context.SaveChanges(); // Sauvegarder les changements

                    MessageBox.Show("Le paiement a été ajouté avec succès.",
                                    "Succès", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Envoyer l'email de confirmation
                    SendConfirmationEmail(res.Client, res);
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
            FilterReservations();
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

        public void SaveReservation()
        {
            if (!CanSaveReservation())
            {
                return;
            }


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

                LoadData();
                OnPropertyChanged(nameof(Reservations));


            }

        }




        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void OpenEditReservationWindow(Reservation reservation)
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
                    reservationToUpdate.Price = (reservation.CheckOutDate - reservation.CheckInDate).Days * reservation.Room.RoomType.Price;

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

        /*exporter*/
        public void ExportReservationsToExcel(string filePath)
        {
            if (Reservations == null || !Reservations.Any())
            {
                MessageBox.Show("Aucun reservation à exporter.", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                // Activer le support de licence pour EPPlus
                ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

                // Créer un nouveau fichier Excel
                using (var package = new ExcelPackage())
                {
                    var worksheet = package.Workbook.Worksheets.Add("Reservations");

                    // Ajouter l'en-tête
                    worksheet.Cells[1, 1].Value = "ID";
                    worksheet.Cells[1, 2].Value = "Date";
                    worksheet.Cells[1, 3].Value = "CheckInDate";
                    worksheet.Cells[1, 4].Value = "CheckOutDate";
                    //worksheet.Cells[1, 5].Value = "FullName";
                    //worksheet.Cells[1, 6].Value = "RoomNumber";
                    worksheet.Cells[1, 5].Value = "Price";
                    worksheet.Cells[1, 6].Value = "Status";

                    // Appliquer un style à l'en-tête
                    using (var range = worksheet.Cells[1, 1, 1, 6])
                    {
                        range.Style.Font.Bold = true;
                        range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                        range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                        range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    }

                    // Remplir les données des reservations
                    int row = 2;
                    foreach (var reservation in _context.Reservations.OrderBy(r => r.ID))
                    {
                        worksheet.Cells[row, 1].Value = reservation.ID;
                        worksheet.Cells[row, 2].Value = reservation.Date;
                        worksheet.Cells[row, 3].Value = reservation.CheckInDate;
                        worksheet.Cells[row, 4].Value = reservation.CheckOutDate;
                        //worksheet.Cells[row, 6].Value = reservation.Client.FullName;
                        //worksheet.Cells[row, 7].Value = reservation.Room.Number;
                        worksheet.Cells[row, 5].Value = reservation.Price;
                        worksheet.Cells[row, 6].Value = reservation.Status;


                        // Appliquer le format de date aux cellules de date
                        worksheet.Cells[row, 2].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet.Cells[row, 3].Style.Numberformat.Format = "dd/MM/yyyy";
                        worksheet.Cells[row, 4].Style.Numberformat.Format = "dd/MM/yyyy";
                        row++;
                    }

                    // Ajuster les colonnes automatiquement
                    worksheet.Cells.AutoFitColumns();
                    // Sauvegarder le fichier Excel
                    File.WriteAllBytes(filePath, package.GetAsByteArray());
                }
                MessageBox.Show("Exportation terminée avec succès !", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'exportation : {ex.Message}", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ExportReservations()
        {
            // Ouvrir un dialog pour sélectionner le chemin de sauvegarde
            var saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Reservations",
                DefaultExt = ".xlsx",
                Filter = "Excel files (*.xlsx)|*.xlsx"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                ExportReservationsToExcel(saveFileDialog.FileName);
            }
        }
        private bool CanExportReservations()
        {
            return Reservations != null && Reservations.Any();
        }

       


      

        private void SendConfirmationEmail(Client client, Reservation reservation)
        {

            if (client == null)
            {
                MessageBox.Show("Client information is missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (reservation == null)
            {
                MessageBox.Show("Reservation information is missing.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
               
                var fromAddress = new MailAddress("radouane.aitsaid01@gmail.com", "Hotel Menara");
                var toAddress = new MailAddress(client.Email, client.FullName);
                const string fromPassword = "ckkl jybd lxyc qrry";
                const string subject = "Confirmation de paiement";
                string body = $"Bonjour {client.FullName},\n\n" +
                              $"Nous vous confirmons la réception de votre paiement pour la réservation suivante :\n" +
                              $"Réservation ID : {reservation.ID}\n" +
                              $"Chambre : {reservation.Room.Number}\n" +
                              $"Date d'arrivée : {reservation.CheckInDate:dd/MM/yyyy}\n" +
                              $"Date de départ : {reservation.CheckOutDate:dd/MM/yyyy}\n" +
                              $"Montant payé : {reservation.Price} €\n\n" +
                              "Merci pour votre confiance.\n" +
                              "Cordialement,\n" +
                              "Menara Hotel";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new System.Net.NetworkCredential(fromAddress.Address, fromPassword)
                };

                using (var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = subject,
                    Body = body
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erreur lors de l'envoi de l'email : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        
    






}
}

