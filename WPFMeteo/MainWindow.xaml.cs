using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.IO;
using Newtonsoft.Json;
//using System.Web.Configuration;

namespace WPFMeteo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            comboBoxCities.Items.Add("Moscow,ru");
            comboBoxCities.Items.Add("London,uk");
            comboBoxCities.Items.Add("Berlin,de");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RequestClass request = new RequestClass("https://localhost:44316/api/city/get?name=", city.Text);
            var req = request.MakeGetRequest(request.Url, request.NameCity);
            var resp = request.MakeGetResponse(req);
            City cityConvert = JsonConvert.DeserializeObject<City>(resp);
            resultId.Text = cityConvert.Id.ToString();
            resultTemp.Text = cityConvert.Temperature.ToString();
            resultCoun.Text = cityConvert.Country;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var request = new RequestClass("https://api.kanye.rest?format=text");
            var req = request.MakeGetRequest(request.Url);
            var resp = request.MakeGetResponse(req);
            textResultGet.Items.Clear();
            textResultGet.Items.Add(resp);           
        }

        private void AddCity_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestClass("https://localhost:44316/api/city/AddCity");
            var req = request.MakeGetRequest(request.Url);
            City city = new City(Convert.ToInt32(setId.Text),
                setName.Text,
                Convert.ToDouble(setTemp.Text),
                setCoun.Text);

            request.MakePostReq(req, city);
            
            outInfo.Text = "Город успешно добавлен в БД!";
        }

        private void AddWeather_Click(object sender, RoutedEventArgs e)
        {
            var request = new RequestClass("http://api.openweathermap.org/data/2.5/weather?q=" + comboBoxCities.SelectedItem + "&APPID=da4699b14a6cb7aec0fe2f8899ed00e7");
            var req = request.MakeGetRequest(request.Url);
            var str = request.GetWeatherCity(req);

            listWeather.Items.Add(str);
            OpenWeather openweather = JsonConvert.DeserializeObject<OpenWeather>(str);

            textOvercast.Text = openweather.clouds.all.ToString(); //Облачность.
            textPressure.Text = openweather.main.pressure.ToString(); //Давление.
            textWind.Text = openweather.wind.speed.ToString(); //Скорость ветра.
            textTempN.Text = Math.Round(Convert.ToDecimal(openweather.main.temp - 273), 2).ToString(); 
            textTempMin.Text = Math.Round(Convert.ToDecimal(openweather.main.temp_min - 273), 2).ToString(); 
            textTempMax.Text = Math.Round(Convert.ToDecimal(openweather.main.temp_max - 273), 2).ToString(); 
        }

    }
}
