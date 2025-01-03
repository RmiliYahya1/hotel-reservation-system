using System.ComponentModel;
using System.Runtime.CompilerServices;
using hotel_reservation_DAL.Contexts;
using LiveCharts;

namespace hotel_reservation_desktop_app.ViewModels;

public class DashbordViewModel: INotifyPropertyChanged
{
    private int _totalClients;
    private int _totalReservations;
    private int _totalPaidReservations;
    private int _totalRooms;
    private int _availableRooms;
    private int _reservationsThisMonth;

    private ChartValues<double> _monthlyProfits;

    public ChartValues<double> MonthlyProfits
    {
        get => _monthlyProfits;
        set
        {
            _monthlyProfits = value;
            OnPropertyChanged();
        }
    }

    private ChartValues<int> _monthlyReservations;

    public ChartValues<int> MonthlyReservations
    {
        get => _monthlyReservations;
        set
        {
            _monthlyReservations = value;
            OnPropertyChanged();
        }
    }
    public int TotalClients
    {
        get => _totalClients;
        set
        {
            _totalClients = value;
            OnPropertyChanged();
        }
    }

    public int TotalReservations
    {
        get => _totalReservations;
        set
        {
            _totalReservations = value;
            OnPropertyChanged();
        }
    }

    public int TotalPaidReservations
    {
        get => _totalPaidReservations;
        set
        {
            _totalPaidReservations = value;
            OnPropertyChanged();
        }
    }

    public int TotalRooms
    {
        get => _totalRooms;
        set
        {
            _totalRooms = value;
            OnPropertyChanged();
        }
    }

    public int AvailableRooms
    {
        get => _availableRooms;
        set
        {
            _availableRooms = value;
            OnPropertyChanged();
        }
    }

    public int ReservationsThisMonth
    {
        get => _reservationsThisMonth;
        set
        {
            _reservationsThisMonth = value;
            OnPropertyChanged();
        }
    }
    
    // Constructor: Load data
    public DashbordViewModel()
    {
        LoadData();
    }

    private void LoadData()
    {
        using (var context = new HotelReservationContext())
        {
            TotalClients = context.Clients.Count();
            TotalReservations = context.Reservations.Count();
            TotalPaidReservations = context.Reservations.Count(r => r.PaymentId != null);
            TotalRooms = context.Rooms.Count();
            AvailableRooms = context.Rooms.Count(r => r.IsAvailable); // Exemple
            ReservationsThisMonth = context.Reservations.Count(r => r.Date.Month == DateTime.Now.Month);

            // Calcul des réservations par mois
            var monthlyReservations = new ChartValues<int>();
            for (int i = 1; i <= 12; i++)
            {
                int count = context.Reservations
                    .Count(r => r.Date.Month == i && r.Date.Year == DateTime.Now.Year);
                monthlyReservations.Add(count);
            }
            MonthlyReservations = monthlyReservations;

            // Calcul des profits par mois (si nécessaire)
            var monthlyProfits = new ChartValues<double>();
            for (int i = 1; i <= 12; i++)
            {
                double monthProfit = context.Reservations
                    .Where(r => r.Date.Month == i && r.Date.Year == DateTime.Now.Year)
                    .Sum(r => (double?)r.Price) ?? 0; // Calcul du profit pour chaque mois
                monthlyProfits.Add(monthProfit);
            }
            MonthlyProfits = monthlyProfits;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}