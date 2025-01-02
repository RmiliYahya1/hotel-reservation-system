using System.Windows;
using System.Windows.Controls;
using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.gestionRoomType;

public partial class AjouterRoomType : Window
{
    private RoomTypeViewModel _roomTypeViewModel;
    public AjouterRoomType(RoomTypeViewModel roomTypeViewModel)
    {
        InitializeComponent();
        _roomTypeViewModel = roomTypeViewModel;
    }
    
    
    private void AjouterButton_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            // Récupérer les données des TextBox
            string name = NomTxt.Text;
            string description = DescriptionTxt.Text;
            double price = double.Parse(PrixTxt.Text);
            int capacity = int.Parse(CapaciteTxt.Text);

            // Créer un nouvel objet RoomType
            var newRoomType = new RoomType
            {
                Name = name,
                Description = description,
                Price = price,
                Capacity = capacity
            };

            // Ajouter via le ViewModel
            _roomTypeViewModel.AjouterRoomType(newRoomType);

            MessageBox.Show("RoomType ajouté avec succès !");
            Close();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur : {ex.Message}");
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        Window.GetWindow(this)?.Close();
    }
    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(NomTxt.Text))
        {
            TextBlock.Visibility= Visibility.Visible;
        }
        else
        {
            TextBlock.Visibility = Visibility.Hidden;
        }
    }
}