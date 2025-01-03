using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace hotel_reservation_desktop_app.ViewModels
{
    interface IUserRepository
    {
        bool AuthentificateUser(NetworkCredential credential);
    }
}
