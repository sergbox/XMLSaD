using System;
using System.Collections.Generic;
using System.Windows;
using System.Xml.Serialization;
using System.IO;
using XMLSaD.DataModel;
using XMLSaD.Classes;
using XMLSaD.CustomControls;

namespace XMLSaD.Windows
{
    /// <summary>
    /// Логика взаимодействия для ShowWeatherAtTimeWindow.xaml
    /// </summary>
    public partial class ShowWeatherAtTimeWindow : Window
    {
        private Prognosis prognosis;
        public ShowWeatherAtTimeWindow(Prognosis prog)
        {
            InitializeComponent();
            prognosis = prog;
            labelContent.Content += " " + prognosis.Date.Value.ToShortDateString();
            LoadDataWrapPanel(prog.Time);
        }
        private void LoadDataWrapPanel(string info) 
        {
            List<ComplexWeatherInfo> infos;
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<ComplexWeatherInfo>));
            using (StringReader reader = new StringReader(info)) 
            {
                infos = xmlSerializer.Deserialize(reader) as List<ComplexWeatherInfo>;
            }
            infos.ForEach(item =>wrapPanelWeather.Children.Add(new WeatherCustomControl(item)));
        }
    }
}
