using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;


namespace hotel_reservation_desktop_app.ViewModels;

public class ClientViewModel:INotifyPropertyChanged
{
    private string _nom;
    private string _prenom;
    private string _telephone;
    private string _email;
    private string _cin;
    private readonly HotelReservationContext _context;
    private Window _window;
    public RelayCommand AjouterClientComand { get;}
    public ClientViewModel()
    {
        _context = new HotelReservationContext();
       
        AjouterClientComand = new RelayCommand(AjouterClient, CanAjouterClient);
        NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
        PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
        LoadClients(1); 
       
    }
    public ClientViewModel(Window window)
    {
       _context = new HotelReservationContext();
       _window = window;
        AjouterClientComand = new RelayCommand(AjouterClient, CanAjouterClient);
        NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
        PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
        LoadClients(1); 
       
    }
    public string Nom
    {
        get { return _nom;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Nom));
            
        }
    }
    public string Prenom
    {
        get { return _prenom; }
        set
        {
            _prenom=value;
            OnPropertyChanged(nameof(Prenom));
        }
    }
    public string Telephone
    {
        get { return _telephone; }
        set
        {
            _telephone=value;
            OnPropertyChanged(nameof(Telephone));
        }
    }
    public string Email
    {
        get { return _email; }
        set
        {
            _email=value;
            OnPropertyChanged(nameof(Email));
        }
    }
    public string CIN
    {
        get { return _cin; }
        set
        {
            _cin=value;
            OnPropertyChanged(nameof(CIN));
        }
    }

   /*Ajout de client*/
    private void AjouterClient()
    {
        Client newClient = new Client();
        newClient.FirstName = Nom;
        newClient.LastName = Prenom;
        newClient.Email = Email;
        newClient.Cin = CIN;
        newClient.PhoneNumber = Telephone;
        _context.Clients.Add(newClient);
        _context.SaveChanges();
        _window.Close();
    }
    private bool CanAjouterClient()
    {
        return true;
    }
    
    
    /*Pagination*/
    private ObservableCollection<Client> _clients;
    private int _currentPage;
    private int _totalPages;
    private const int PageSize = 10;
    public ObservableCollection<Client> Clients
    {
        get => _clients;
        set
        {
            _clients = value;
            OnPropertyChanged(nameof(Clients));
        }
    }
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            _currentPage = value;
            OnPropertyChanged(nameof(CurrentPage));
        }
    }
    public int TotalPages
    {
        get => _totalPages;
        set
        {
            _totalPages = value;
            OnPropertyChanged(nameof(TotalPages));
        }
    }
    public RelayCommand NextPageCommand { get; }
    public RelayCommand PreviousPageCommand { get; }
    private void LoadClients(int nombrePage)
    {
        if (nombrePage < 1)
            nombrePage = 1;

        var totalClients = _context.Clients.Count();
        TotalPages = (int)Math.Ceiling((double)totalClients / PageSize);

        if (nombrePage > TotalPages && TotalPages > 0)
            nombrePage = TotalPages;

        Clients = new ObservableCollection<Client>(
            _context.Clients
                .OrderBy(c => c.ID) 
                .Skip((nombrePage - 1) * PageSize)
                .Take(PageSize)
                .ToList()
        );

        CurrentPage = nombrePage;
    }
    private void NextPage()
    {
        if (CurrentPage < TotalPages)
        {
            LoadClients(CurrentPage + 1);
        }
    }
    private bool CanGoToNextPage()
    {
        return true;
    }
    private void PreviousPage()
    {
        if (CurrentPage > 1)
        {
            LoadClients(CurrentPage - 1);
        }
    }
    private bool CanGoToPreviousPage()
    {
        return true;
    }

    /*Delete client*/
    public void RemoveClient(int clientId)
    {
        var clientToRemove = _context.Clients.FirstOrDefault(c => c.ID == clientId);
        if (clientToRemove != null)
        {
            _context.Clients.Remove(clientToRemove);
            _context.SaveChanges(); 
            LoadClients(CurrentPage); 
        }
    }
    /*Proprety change*/
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}