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

namespace StatsApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        GeneralInfo.NavigationUIVisibility = NavigationUIVisibility.Hidden;

        GeneralInfo.Navigate(new GeneralInfo(),false);
    }

    public void LoadGeneralInfo()
    {
        GeneralInfo.Content = new GeneralInfo();
    }
    public void LoadStatsReport()
    {
        GeneralInfo.Content = new StatsReport();
    }
}