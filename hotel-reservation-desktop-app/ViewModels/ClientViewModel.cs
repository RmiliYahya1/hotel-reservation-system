using System.ComponentModel;
using System.Windows;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.View.gestionClient;

namespace hotel_reservation_desktop_app.ViewModels;

public class ClientViewModel:INotifyPropertyChanged
{
    private string _nom;

    public string Nom
    {
        get { return _nom;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Nom));
            
        }
    } 
    private string _prenom;

    public string Prenom
    {
        get { return _prenom;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Prenom));
            
        }
    } 
    
    private string _telephone;

    public string Telephone
    {
        get { return _telephone;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Telephone));
            
        }
    }
    
    private string _email;

    public string Email
    {
        get { return _email;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Email));
            
        }
    } 
    
    private string _cin;

    public string Cin
    {
        get { return _cin;}
        set
        {
            _nom=value;
            OnPropertyChanged(nameof(Cin));
            
        }
    }
    
    public event PropertyChangedEventHandler PropertyChanged;
    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}