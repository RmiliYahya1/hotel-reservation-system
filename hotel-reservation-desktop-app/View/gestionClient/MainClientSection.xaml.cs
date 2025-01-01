using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.View.gestionClient;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.view.gestionClient;

public partial class MainClientSection
{
    private ClientViewModel clientViewModel; 
    public MainClientSection()
    {
        InitializeComponent(); 
        clientViewModel = new ClientViewModel();
        DataContext = clientViewModel;
    }
    
    
    private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null && !(current is T))
        {
            current = VisualTreeHelper.GetParent(current);
        }
        return current as T;
    }
    
    
    
    //évenement de suppression
    private void Supprimer(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            var row = FindAncestor<DataGridRow>(button);
            if (row != null)
            {
                Client client = row.DataContext as Client;
                if (client != null)
                {
                    MessageBoxResult result = MessageBox.Show(
                        "Êtes-vous sûr de vouloir supprimer ce client ?", 
                        "Confirmation", 
                        MessageBoxButton.YesNo, 
                        MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        clientViewModel.RemoveClient(client.ID);
                    }
                }
            }
        }
    }
    
    
    
    //évenement d'ajout
    private void Ajouter(object sender, RoutedEventArgs e)
    {
        AjoutClient ajoutClient = new AjoutClient();
        ajoutClient.ShowDialog();
    }
    
    
}