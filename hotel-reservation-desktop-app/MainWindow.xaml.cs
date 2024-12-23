using hotel_reservation_DAL.Entities;
using hotel_reservation_desktop_app.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace hotel_reservation_desktop_app;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private UserViewModel _viewModel;

    public MainWindow()
    {
        InitializeComponent();
        _viewModel = new UserViewModel();
        DataContext = _viewModel;
    }

    private void AddUser_Click(object sender, RoutedEventArgs e)
    {
        var user = new User
        {
            Username = UsernameBox.Text,
            Email = EmailBox.Text,
            PasswordHash = PasswordBox.Text,
            Role = RoleBox.Text
        };
        _viewModel.AddUser(user);
    }
}
