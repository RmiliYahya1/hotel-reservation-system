using hotel_reservation_desktop_app.ViewModels;

namespace hotel_reservation_desktop_app.View.dashbord;

public partial class Dashbord 
{
    public Dashbord()
    {
        InitializeComponent();
        DataContext = new DashbordViewModel();
    }
}