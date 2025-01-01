using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class GestionRoomViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Room> Rooms { get; set; } = new();
        public ObservableCollection<RoomType> RoomTypes { get; set; } = new();
        public Room RoomSelectionnee { get; set; }
        public ObservableCollection<Room> AllRooms { get; set; } = new(); // Liste complète des chambres
        private string _searchText;

        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));  // Déclenche l'événement de changement
                    LoadRooms();  // Recharger les chambres à chaque modification du texte
                }
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // Propriétés de pagination
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 10;  // Nombre d'éléments par page

        public GestionRoomViewModel()
        {
            LoadRoomTypes();
            LoadRooms(); // Charger les chambres dès le début
        }

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
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                query = query.Where(r => r.Number.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ||
                                         r.RoomType.Name.Contains(SearchText, StringComparison.OrdinalIgnoreCase));
            }

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

    }
}
