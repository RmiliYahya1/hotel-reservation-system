using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hotel_reservation_desktop_app.ViewModels
{
    public class UserViewModel
    {
        public ObservableCollection<User> Users { get; set; } = new();

        public UserViewModel()
        {
            LoadUsers();

        }

        public void LoadUsers()
        {
            using var context = new HotelReservationContext();
            var users = context.Users.ToList();

            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(user);
            }

        }

        public void AddUser(User user)
        {
            using var context = new HotelReservationContext();
            context.Users.Add(user);
            context.SaveChanges();
            
            Users.Add(user);
        }
    }
}
