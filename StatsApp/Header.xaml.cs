using System.Diagnostics;
using System.Net.Mime;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;
namespace StatsApp;

public partial class Header : UserControl
{
    public Header()
    {
        InitializeComponent();
    }

    private void InitialPage(object sender, RoutedEventArgs e)
    {
        
        Console.WriteLine("AAAAA");

        MainWindow main = Application.Current.MainWindow as MainWindow;
        main.LoadGeneralInfo();
    }
    private void StatsPage(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("AAAAA");
        MainWindow main = Application.Current.MainWindow as MainWindow;
        main.LoadStatsReport();
    }
    
}