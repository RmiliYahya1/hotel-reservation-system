using System.Windows;
using hotel_reservation_DAL.Contexts;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionClient;

public partial class AjoutClient : Window
{
    public AjoutClient(Client client = null)
    {
        ClientViewModel clientViewModel = new ClientViewModel(this);
        DataContext = clientViewModel;
        InitializeComponent();
        if (client != null)
        {
            clientViewModel.Nom = client.FirstName;
            clientViewModel.Prenom = client.LastName;
            clientViewModel.Telephone = client.PhoneNumber;
            clientViewModel.Email = client.Email;
            clientViewModel.CIN = client.Cin;
            clientViewModel.ClientToEdit = client; 
            Button.Content = "Modifier"; 
        }
    }
    
    
    


    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
    
}