using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using hotel_reservation_DAL.Contexts;

namespace hotel_reservation_desktop_app.ViewModels
{
    class UserRepository : HotelReservationContext, IUserRepository
    {
        public bool AuthentificateUser(NetworkCredential credential)
        {
            bool validUser;

            using (var context = new HotelReservationContext())  // Utilisez le DbContext pour l'accès à la base de données
            {
                // Rechercher un utilisateur avec le nom d'utilisateur et le mot de passe (ou hash) dans la base de données
                var user = context.Users
                                  .Where(u => u.Username == credential.UserName && u.PasswordHash == credential.Password)  // Ajustez si vous stockez un hash au lieu du mot de passe en clair
                                  .FirstOrDefault();

                validUser = user != null;  // Si l'utilisateur est trouvé, il est valide, sinon non
            }

            return validUser;
        }
    }
}
