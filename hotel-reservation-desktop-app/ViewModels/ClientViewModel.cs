using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.Services;
using hotel_reservation_desktop_app.View.gestionClient;

namespace hotel_reservation_desktop_app.ViewModels;

public class ClientViewModel:INotifyPropertyChanged
{
    private string _nom;
    private string _prenom;
    private string _telephone;
    private string _email;
    private string _cin;
    private readonly ClientService _clientService;
   

    public ClientViewModel()
    {
        _clientService = new ClientService();
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
    
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}