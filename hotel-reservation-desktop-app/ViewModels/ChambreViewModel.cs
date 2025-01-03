using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace hotel_reservation_desktop_app.ViewModels;

public class ChambreViewModel: INotifyPropertyChanged
{
    public ObservableCollection<Room> Rooms { get; set; } = new();
    private readonly HotelReservationContext _context;
 
    public ObservableCollection<RoomType> RoomTypes { get; set; } = new();
    public Room RoomSelectionnee { get; set; }
    public ObservableCollection<Room> AllRooms { get; set; } = new(); // Liste complète des chambres
  
       
   
  
   
        


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Propriétés de pagination
        private int _currentPage = 1;
        public int CurrentPage
        {
            get{return _currentPage;}
            set
            {
                _currentPage = value;
                OnPropertyChanged(nameof(CurrentPage));
            }
            
        } 
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;  // Nombre d'éléments par page

        public ChambreViewModel()
        {
            _context = new HotelReservationContext();
            LoadRoomTypes();
            LoadRooms(); 
            ExportChambresToExcelCommand = new RelayCommand(ExportChambres, CanExportChambres);
           
        }
        public RelayCommand ExportChambresToExcelCommand { get; }
        

        // Charger les RoomTypes depuis la base de données
        public void LoadRoomTypes()
        {
            using var context = new HotelReservationContext();
            var roomTypes = context.RoomTypes.ToList();

            RoomTypes.Clear();
            foreach (var roomType in roomTypes)
            {
                RoomTypes.Add(roomType);
            }
        }

        // Charger les Rooms depuis la base de données avec la pagination
        // Charger les Rooms depuis la base de données
        public void LoadRooms()
        {
            using var context = new HotelReservationContext();

            // Crée une requête de base pour les chambres, y compris RoomType
            var query = context.Rooms.Include(r => r.RoomType).AsQueryable();

            // Appliquer la recherche si SearchText n'est pas vide
        
            // Calculer le nombre total de chambres après filtrage
            var totalRooms = query.Count();
            TotalPages = (int)Math.Ceiling(totalRooms / (double)PageSize);

            // Appliquer la pagination
            var rooms = query
                .Skip((CurrentPage - 1) * PageSize)  // Passer les chambres des pages précédentes
                .Take(PageSize)  // Prendre les chambres de la page actuelle
                .ToList();

            // Effacer les chambres actuelles dans l'ObservableCollection et les remplacer
            Rooms.Clear();
            foreach (var room in rooms)
            {
                Rooms.Add(room);  // Ajouter chaque chambre avec son RoomType (catégorie)
            }
        }



        // Méthode pour changer de page
        public void GoToPage(int page)
        {
            if (page > 0 && page <= TotalPages)
            {
                CurrentPage = page;
                LoadRooms();
            }
        }

        // Méthode pour aller à la page suivante
        public void NextPage()
        {
            if (CurrentPage < TotalPages)
            {
                CurrentPage++;
                LoadRooms();
            }
        }

        // Méthode pour aller à la page précédente
        public void PreviousPage()
        {
            if (CurrentPage > 1)
            {
                CurrentPage--;
                LoadRooms();
            }
        }

        // Ajouter une chambre
        public void AjouterRoom(Room newRoom)
        {
            using var context = new HotelReservationContext();

            // Ajouter la chambre à la base de données
            context.Rooms.Add(newRoom);
            context.SaveChanges();

            // Récupérer le RoomType correspondant à l'ID de la chambre
            var roomType = context.RoomTypes.FirstOrDefault(rt => rt.ID == newRoom.RoomTypeId);

            // Associer le RoomType à la chambre nouvellement ajoutée
            if (roomType != null)
            {
                newRoom.RoomType = roomType;
            }

            // Ajouter la chambre à la liste observable pour l'affichage
            Rooms.Add(newRoom);
        }

        // Modifier une chambre
        public void ModifierRoom(Room roomToUpdate)
        {
            try
            {
                using var context = new HotelReservationContext();

                // Recherche de la chambre à modifier dans la base de données
                var room = context.Rooms.FirstOrDefault(r => r.Number == roomToUpdate.Number);

                if (room != null)
                {
                    // Mise à jour des propriétés de la chambre
                    room.Number = roomToUpdate.Number;
                    room.IsAvailable = roomToUpdate.IsAvailable;
                    room.RoomTypeId = roomToUpdate.RoomTypeId;

                    // Sauvegarde des modifications dans la base de données
                    context.SaveChanges();

                    // Mise à jour dans la collection ObservableCollection
                    var index = Rooms.IndexOf(Rooms.FirstOrDefault(r => r.Number == roomToUpdate.Number));
                    if (index >= 0)
                    {
                        Rooms[index] = room;
                    }

                    // Affichage d'un message de succès
                    MessageBox.Show("La chambre a été modifiée avec succès.", "Succès", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    // Si la chambre n'a pas été trouvée
                    MessageBox.Show("La chambre spécifiée n'existe pas.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Gestion des erreurs
                MessageBox.Show($"Une erreur est survenue : {ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Supprimer une chambre
        public void SupprimerRoom(Room roomToDelete)
        {
            if (roomToDelete == null) return;

            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer cette chambre ?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using var context = new HotelReservationContext();

                var room = context.Rooms.FirstOrDefault(r => r.ID == roomToDelete.ID);
                if (room != null)
                {
                    // Supprimer la chambre de la base de données
                    context.Rooms.Remove(room);
                    context.SaveChanges();

                    // Supprimer la chambre localement de la collection ObservableCollection
                    Rooms.Remove(roomToDelete);

                    MessageBox.Show("Chambre supprimée avec succès.");
                }
            }
        }
        
        public void ExportClientsToExcel(string filePath) {
             if (Rooms == null || !Rooms.Any()) {
                     MessageBox.Show("Aucune chambre à exporter.", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return; }
             try
            {
      
             ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

      
        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add("Rooms");

           
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Nombre";
            worksheet.Cells[1, 3].Value = "Disponibilité";
            worksheet.Cells[1, 4].Value = "Catégorie";
          

           
            using (var range = worksheet.Cells[1, 1, 1, 6])
            {
                range.Style.Font.Bold = true;
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }

      
            int row = 2;
            foreach (var chambre in _context.Rooms.OrderBy(r => r.ID).Include(room => room.RoomType))
            {
                worksheet.Cells[row, 1].Value = chambre.ID;
                worksheet.Cells[row, 2].Value = chambre.Number;
                worksheet.Cells[row, 3].Value = chambre.IsAvailable;
                worksheet.Cells[row, 4].Value = chambre.RoomType.Name;
                row++;
            }

          
            worksheet.Cells.AutoFitColumns();
           
            File.WriteAllBytes(filePath, package.GetAsByteArray());
        }
        MessageBox.Show("Exportation terminée avec succès !", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Information);
    }
    catch (Exception ex)
    {
        MessageBox.Show($"Erreur lors de l'exportation : {ex.Message}", "Exportation Excel", MessageBoxButton.OK, MessageBoxImage.Error);
    }
}
        
private void ExportChambres()
{
       
    var saveFileDialog = new Microsoft.Win32.SaveFileDialog
    {
        FileName = "Chambres",
        DefaultExt = ".xlsx",
        Filter = "Excel files (*.xlsx)|*.xlsx"
    };

    if (saveFileDialog.ShowDialog() == true)
    {
        ExportClientsToExcel(saveFileDialog.FileName);
    }
}
private bool CanExportChambres()
{
    return Rooms != null && Rooms.Any();
}
        
}