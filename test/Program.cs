using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        InsertData();
        PrintData();
    }

    private static void InsertData()
    {
        //ajouter un user
        using var context = new HotelReservationContext();
        var user = new User
        {
            Username = "admin",
            Email = "red",
            PasswordHash = "12345",
            Role = "admin"
        };
        context.Users.Add(user);
        context.SaveChanges();
    }

    private static void PrintData()
    {
        //afficher les reservations
        using var context = new HotelReservationContext();
        var reservations = context.Reservations
            .Include(r => r.Client)
            .Include(r => r.Room)
            .Include(r => r.Payment)
            .ToList();
        foreach (var reservation in reservations)
        {
            Console.WriteLine($"Reservation ID: {reservation.ID}");
            Console.WriteLine($"Check-in: {reservation.CheckInDate}");
            Console.WriteLine($"Check-out: {reservation.CheckOutDate}");
            Console.WriteLine($"Client: {reservation.Client.FirstName} {reservation.Client.LastName}");
            Console.WriteLine($"Room: {reservation.Room.Number}");
            Console.WriteLine($"Price: {reservation.Price}");
            if (reservation.Payment != null)
            {
                Console.WriteLine($"Payment: {reservation.Payment.Amount} {reservation.Payment.PaymentMethod}");
            }
            Console.WriteLine();
        }
    }

}
