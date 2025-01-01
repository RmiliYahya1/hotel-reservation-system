using System.Windows;
using System.Windows.Controls;

namespace hotel_reservation_desktop_app.View.UserControls;

public partial class MyTextBox : UserControl
{
    public MyTextBox()
    {
        InitializeComponent();
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }
    
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(
            nameof(Text), 
            typeof(string), 
            typeof(MyTextBox), 
            new FrameworkPropertyMetadata(
                null, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault 
            ));
    
    public string Caption
    {
        get { return (string)GetValue(CaptionProperty); }
        set { SetValue(CaptionProperty, value); }
    }
    
    public static readonly DependencyProperty CaptionProperty =
        DependencyProperty.Register(
            nameof(Caption), 
            typeof(string), 
            typeof(MyTextBox), 
            new FrameworkPropertyMetadata(
                null, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault 
            ));

    
    public string Hint
    {
        get { return (string)GetValue(HintProperty); }
        set { SetValue(HintProperty, value); }
    }
    
    public static readonly DependencyProperty HintProperty =
        DependencyProperty.Register(
            nameof(Hint), 
            typeof(string), 
            typeof(MyTextBox), 
            new FrameworkPropertyMetadata(
                null, 
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault 
            ));

    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (string.IsNullOrEmpty(TextBox.Text))
        {
            TextBlock.Visibility= Visibility.Visible;
        }
        else
        {
            TextBlock.Visibility = Visibility.Hidden;
        }
    }
}