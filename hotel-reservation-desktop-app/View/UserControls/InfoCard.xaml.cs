using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using hotel_reservation_desktop_app.view.gestionClient;

namespace hotel_reservation_desktop_app.View.UserControls;

public partial class InfoCard : UserControl
{
    public InfoCard()
    {
        InitializeComponent();
    }

    public string Titre
    {
        get { return (string)GetValue(TitreProprety); }
        set{SetValue(TitreProprety,value);}
    }
    
    public static readonly DependencyProperty TitreProprety = 
        DependencyProperty.Register("Titre", typeof(string), typeof(InfoCard));
    
    
     public string Number
    {
        get { return (string)GetValue(NumberProprety); }
        set{SetValue(NumberProprety,value);}
    }
    
    public static readonly DependencyProperty NumberProprety = 
        DependencyProperty.Register("Number", typeof(string), typeof(InfoCard));
    
    
     public string IsActive
    {
        get { return (string)GetValue(IsActiveProprety); }
        set{SetValue(IsActiveProprety,value);}
    }
    
    public static readonly DependencyProperty IsActiveProprety = 
        DependencyProperty.Register("IsActive", typeof(string), typeof(InfoCard));
    
     public string IsTechnical
    {
        get { return (string)GetValue(IsTechnicalProprety); }
        set{SetValue(IsTechnicalProprety,value);}
    }
    
    public static readonly DependencyProperty IsTechnicalProprety = 
        DependencyProperty.Register("IsTechnical", typeof(string), typeof(InfoCard));

    
}