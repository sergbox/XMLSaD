using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using XMLSaD.DataModel;
using XMLSaD.Classes;
using System.IO;

namespace XMLSaD.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddWeatherWindow.xaml
    /// </summary>
    public partial class AddWeatherWindow : Window
    {
        private ModelEF model = new ModelEF();
        private List<ComplexWeatherInfo> listweather = new List<ComplexWeatherInfo>(); 
        private DateTime? selectdate;
        public AddWeatherWindow()
        {
            InitializeComponent();
            ComboBoxDataLoad();
            calendar1.SelectedDate = DateTime.Now.Date;
            selectdate = calendar1.SelectedDate;
        }
        private void ComboBoxDataLoad() 
        {
            comboboxTypes.Items.Clear();
            model.Type_of_weather.ToList().ForEach(type => comboboxTypes.Items.Add(type.Name));
            comboboxTypes.SelectedIndex = 0;
            comboboxTime.Items.Clear();
            for (int i = 0; i < 24; i++)
                comboboxTime.Items.Add((i < 10 ? $"0{i}" : $"{i}") + ":00");
            comboboxTime.SelectedIndex = 0;
        }
        private Prognosis CreatePrognosis()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ComplexWeatherInfo>));
            string serializedXML;
            using (StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, listweather.OrderBy(x => x.Time).ToList()); 
                serializedXML = writer.ToString();
            }
            return new Prognosis(calendar1.SelectedDate, serializedXML);
        }
        private void UpdatePrognosis(ref Prognosis prognosis) 
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ComplexWeatherInfo>)); 
            string serializedXML;
            using (StringWriter writer = new StringWriter())
            {
                xmlSerializer.Serialize(writer, listweather.OrderBy(x => x.Time).ToList()); 
                serializedXML = writer.ToString();
            }
            prognosis.Date = calendar1.SelectedDate;
            prognosis.Time = serializedXML;
        }
        private void RemoveSelect()
        {
            listweather = new List<ComplexWeatherInfo>();
            comboboxTime.SelectedIndex = 0;
        }
        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            if (listweather.Count == 0) 
            {
                MessageBox.Show("Добавьте данные!");
                return;
            }
            Prognosis prognosis = model.Prognosis.FirstOrDefault(x => x.Date == calendar1.SelectedDate);
            if (prognosis != null)
            {
                MessageBoxResult messageBox = MessageBox.Show("Ha выбранну дату уже найдена запись!\n" + "Xотите перезаписать еë?", "Сoобшение", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                switch (messageBox)
                {
                    case MessageBoxResult.Yes:
                        UpdatePrognosis(ref prognosis);
                        break;
                    case MessageBoxResult.No:
                        return;
                    case MessageBoxResult.Cancel:
                        return;
                }
            }
            else
            { model.Prognosis.Add(CreatePrognosis()); }
            try { model.SaveChanges(); }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Данные сохранены"); 
            RemoveSelect();
        }
        private void buttonShowWeather_Click(object sender, RoutedEventArgs e)
        {
            if (listweather.Count == 0)
            {
                MessageBox.Show($"Погодa нa {calendar1.SelectedDate.Value.ToShortDateString()} не добавлена!");
                return;
            }
            ShowWeatherAtTimeWindow window = new ShowWeatherAtTimeWindow(CreatePrognosis()); 
            window.ShowDialog();
        }
        private void buttonBack_Click(object sender, RoutedEventArgs e)
        { 
            Close();
        }
        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (!double.TryParse(textBoxTemperature.Text, out double n1))
            {
                MessageBox.Show("В поле Температура данные вводятся в вещественном формате!");
                return;
            }
            if (!double.TryParse(textBoxHumidity.Text, out double n2))
            {
                MessageBox.Show("В поле Влажность данные вводятся в вещественном формате!");
                return;
            }
            if (double.Parse(textBoxHumidity.Text) > 100 || double.Parse(textBoxHumidity.Text) < 0)
            {
                MessageBox.Show("В поле Влажность можно вводить данные от 0 до 100!");
                // Занесение данных в объект класса ComplexWeatherInfo при его помощи конструктора
                ComplexWeatherInfo complexWeatherInfo = new ComplexWeatherInfo(
                comboboxTime.SelectedValue.ToString(),
                    model.Type_of_weather.First(x => comboboxTypes.SelectedItem.ToString() == x.Name), 
                    double.Parse(textBoxTemperature.Text), 
                    double.Parse(textBoxHumidity.Text));
                // проверка добавлялись ли элементы с одинаковым временем в listweather 
                // если добавлялись то автоматически заменяим их на новые
                for (int i = 0; i < listweather.Count; i++)
                {
                    if (listweather[i].Time == complexWeatherInfo.Time)
                    {
                        listweather[i] = complexWeatherInfo;
                        return;
                    }
                }
                listweather.Add(complexWeatherInfo);
                return;
            }
        }
        private void calendar1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (selectdate != calendar1.SelectedDate && listweather.Count != 0)
            {
                MessageBoxResult messageBox = MessageBox.Show("При изменении даты не сохранённые данные удалятся!\n" + "Вы действительно хотите выбрать новую дату?", "Сообщение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (MessageBoxResult.No == messageBox)
                {
                    calendar1.SelectedDate = selectdate;
                }
                else if (MessageBoxResult.Yes == messageBox)
                {
                    listweather = new List<ComplexWeatherInfo>();
                    selectdate = calendar1.SelectedDate;
                }

            }
        }
    }
}
