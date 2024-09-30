using System.Linq;
using System.Windows;
using XMLSaD.DataModel;
using XMLSaD.CustomControls;

namespace XMLSaD.Windows
{
    /// <summary>
    /// Логика взаимодействия для ShowWeatherWindow.xaml
    /// </summary>
    public partial class ShowWeatherWindow : Window
    {
        public ShowWeatherWindow()
        {
            InitializeComponent();
            LoadDate();
        }
        private void LoadDate() =>
            new ModelEF().Prognosis.ToList().ForEach(item => 
            wrapPanelWeather.Children.Add(new PrognosisWeather(item)));
        private void buttonBack_Click(object sender, RoutedEventArgs e) => Close();
    }
}
