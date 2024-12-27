using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;

namespace hotel_reservation_desktop_app.Services;

public class ClientService
{
 private readonly   HotelReservationContext _context;

 public ClientService()
 {
  HotelReservationContext _context=new HotelReservationContext();
 }

 public void addClient(Client client)
 {
  if (client != null)
  {
   _context.Add(client);
   _context.SaveChanges();
  }
 }
 public void DeleteClient(Client client)
 {
  if (client != null)
  {
   _context.Clients.Remove(client);
   _context.SaveChanges(); 
  }
 }
 
}