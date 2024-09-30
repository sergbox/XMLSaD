using System.Windows;
using XMLSaD.Windows;

namespace XMLSaD.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void buttonAddWeather_Click(object sender, RoutedEventArgs e)
        {
            AddWeatherWindow addWeatherWindow = new AddWeatherWindow();
            addWeatherWindow.ShowDialog();
        }
        private void buttonShowWeather_Click(object sender, RoutedEventArgs e)
        {
            ShowWeatherWindow showWeather = new ShowWeatherWindow();
            showWeather.ShowDialog();
        }
    }
}
