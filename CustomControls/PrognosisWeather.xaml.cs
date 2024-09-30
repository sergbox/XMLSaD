using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using XMLSaD.DataModel;
using XMLSaD.Windows;

namespace XMLSaD.CustomControls
{
    /// <summary>
    /// Логика взаимодействия для PrognosisWeather.xaml
    /// </summary>
    public partial class PrognosisWeather : UserControl
    {
        private Prognosis _thisprognosis;
        public Prognosis prognosis
        {
            get { return _thisprognosis; }
            set
            {
                _thisprognosis = value ?? throw new ArgumentNullException(nameof(_thisprognosis));
            }
        }
        public PrognosisWeather(Prognosis progn)
        {
            InitializeComponent();
            prognosis = progn;
            Fill();
        }
        private void Fill() 
        {
            label_ID.Content = prognosis.ID;
            label_Date.Content = prognosis.Date.Value.ToShortDateString();
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult messageBox = MessageBox.Show(
                $"Вы действительно хотите удалить запись в ID - {prognosis.ID}", "Сообшение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (MessageBoxResult.Yes == messageBox) {
                ModelEF model = new ModelEF();
                model.Prognosis.Remove(model.Prognosis.First(x =>
                _thisprognosis.ID == x.ID));
                try
                {
                    model.SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                this.Visibility = Visibility.Hidden; 
            }
        }

        private void buttonShowWeather_Click(object sender, RoutedEventArgs e)
        {
            ShowWeatherAtTimeWindow window = new ShowWeatherAtTimeWindow(prognosis);
            window.ShowDialog();
        }

        private void Grid_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Button_Delete.Visibility = Visibility.Visible;
        }

        private void PrognosisWeatherControl_MouseLeave(object sender, MouseEventArgs e)
        {
            Button_Delete.Visibility = Visibility.Hidden;
        }
    }
}
