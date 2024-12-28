using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Policy;
using System.Text;

class Program
{
    static void Main(string[] args)
    {
        addClient();
    }

    public static void addClient()
    {
        HotelReservationContext context = new HotelReservationContext();
        Client client = new Client();
        client.FirstName = "John";
        client.LastName = "Doe";
        client.Email = "johndoe@gmail.com";
        client.PhoneNumber = "0123456789";
        client.Cin= "123456";
        context.Clients.Add(client);
        context.SaveChanges();
        Console.WriteLine("Client ajouté avec succès !");
    }

}
