using System.Collections.ObjectModel;
using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.ViewModels;

public class RoomTypeViewModel
{
    public ObservableCollection<RoomType> RoomTypes { get; set; } = new();
    public RoomType RoomTypeSelectionnee { get; set; }

    public RoomTypeViewModel()
    {
        LoadRoomTypes();
    }
    
     // Charger les RoomTypes depuis la base de données
        public void LoadRoomTypes()
        {
            using var context = new HotelReservationContext();
            var users = context.RoomTypes.ToList();

            RoomTypes.Clear();
            foreach (var user in users)
            {
                RoomTypes.Add(user);
            }
        }
        // Méthode pour ajouter un RoomType
        public void AjouterRoomType(RoomType newRoomType)
        {
            using var context = new HotelReservationContext();

            // Ajouter à la base de données
            context.RoomTypes.Add(newRoomType);
            context.SaveChanges();

            // Ajouter localement à l'ObservableCollection
            RoomTypes.Add(newRoomType);
        }

        public void ModifierRoomType(RoomType roomType)
        {
            using var context = new HotelReservationContext();

            // Trouver le RoomType à modifier dans la base de données
            var roomTypeToUpdate = context.RoomTypes.FirstOrDefault(rt => rt.Name == roomType.Name);

            if (roomTypeToUpdate != null)
            {
                // Mettre à jour les propriétés
                roomTypeToUpdate.Name = roomType.Name;
                roomTypeToUpdate.Description = roomType.Description;
                roomTypeToUpdate.Price = roomType.Price;
                roomTypeToUpdate.Capacity = roomType.Capacity;

                // Sauvegarder les modifications dans la base de données
                context.SaveChanges();

                // Mettre à jour l'élément dans la collection ObservableCollection
                var index = RoomTypes.IndexOf(RoomTypes.FirstOrDefault(r => r.Name == roomType.Name));
                if (index >= 0)
                {
                    RoomTypes[index] = roomTypeToUpdate;
                }
            }
        }


        // Supprimer un RoomType
        public void SupprimerRoomType(RoomType roomTypeToDelete)
        {
            if (roomTypeToDelete == null) return;

            var result = MessageBox.Show("Êtes-vous sûr de vouloir supprimer ce RoomType ?", "Confirmation", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                using var context = new HotelReservationContext();

                var roomType = context.RoomTypes.FirstOrDefault(r => r.Name == roomTypeToDelete.Name);
                if (roomType != null)
                {
                    context.RoomTypes.Remove(roomType);
                    context.SaveChanges();

                    RoomTypes.Remove(roomTypeToDelete);

                    MessageBox.Show("RoomType supprimé avec succès.");
                }
            }

        }

    
}