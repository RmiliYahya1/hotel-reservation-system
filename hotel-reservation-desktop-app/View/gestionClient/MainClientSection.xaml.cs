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

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        AjoutClient ajoutClient = new AjoutClient();
        ajoutClient.ShowDialog();
    }

    private void ClientSearch_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(clientSearch.Text))
        {
            TextBlock.Visibility= Visibility.Visible;
        }
        else
        {
            TextBlock.Visibility = Visibility.Hidden;
        }
    }

    private void ButtonDelete_OnClick(object sender, RoutedEventArgs e)
    {
        var button = sender as Button;
        if (button != null)
        {
            // Trouve la ligne DataGridRow parent
            var row = FindAncestor<DataGridRow>(button);
            if (row != null)
            {
                // Récupère l'objet Client lié à cette ligne
                var client = row.DataContext as Client; // Assurez-vous que Client est votre classe de données
                if (client != null)
                {
                    // Supprime le client (appel de la méthode)
                    clientViewModel.RemoveClient(client.ID);
                }
            }
        }
    }
    private T FindAncestor<T>(DependencyObject current) where T : DependencyObject
    {
        while (current != null && !(current is T))
        {
            current = VisualTreeHelper.GetParent(current);
        }
        return current as T;
    }
}