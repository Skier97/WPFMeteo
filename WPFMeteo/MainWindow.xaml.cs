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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            WebRequest req = WebRequest.Create("https://localhost:44316/api/city/get?name=" + city.Text);
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            //resultId.Text = sr.
            string resultGet = sr.ReadToEnd();
            sr.Close();
            City cityConvert;
            cityConvert = JsonConvert.DeserializeObject<City>(resultGet);
            resultId.Text = cityConvert.Id.ToString();
            resultTemp.Text = cityConvert.Temperature.ToString();
            resultCoun.Text = cityConvert.Country;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WebRequest req = WebRequest.Create("https://api.kanye.rest?format=text");
            WebResponse resp = req.GetResponse();
            Stream stream = resp.GetResponseStream();
            StreamReader sr = new StreamReader(stream);
            //resultId.Text = sr.
            string resultGet = sr.ReadToEnd();
            sr.Close();
            //resultId.Text = resultGet.;
            textResultGet.Items.Clear();
            textResultGet.Items.Add(resultGet);
        }

        private void AddCity_Click(object sender, RoutedEventArgs e)
        {
            WebRequest request = WebRequest.Create("https://localhost:44316/api/city/AddCity");
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            City city = new City(Convert.ToInt32(setId.Text),
                setName.Text,
                Convert.ToDouble(setTemp.Text),
                setCoun.Text);
            var jsonData = JsonConvert.SerializeObject(city);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            WebResponse response = request.GetResponse();
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);
            using (dataStream = response.GetResponseStream())
            { 
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                Console.WriteLine(responseFromServer);
            }
            response.Close();
            outInfo.Text = "Город успешно добавлен в БД!";
        }

        private void AddWeather_Click(object sender, RoutedEventArgs e)
        {

            WebRequest req = WebRequest.Create(@"http://api.openweathermap.org/data/2.5/weather?q=Moscow&APPID=a5ca6c5692978d29d84474e9f351648c");
            req.Method = "POST";
            req.ContentType = "application/x-www-urlencoded";

            string str = "";
            OpenWeather openweather;

            WebResponse response = req.GetResponse();
            using (Stream s = response.GetResponseStream()) 
            {
                using (StreamReader r = new StreamReader(s)) 
                {
                    str = r.ReadToEnd(); 
                }
            }
            response.Close(); 
            listWeather.Items.Add(str); 
            openweather = JsonConvert.DeserializeObject<OpenWeather>(str);

            textOvercast.Text = openweather.clouds.all.ToString(); //Облачность.
            textPressure.Text = openweather.main.pressure.ToString(); //Давление.
            textWind.Text = openweather.wind.speed.ToString(); //Скорость ветра.
            textTempN.Text = openweather.main.temp.ToString(); //Тепмпература в фарингейтах сейчас. 
            textTempMin.Text = openweather.main.temp_min.ToString(); //Тепмпература в фарингейтах min на сегодня. 
            textTempMax.Text = openweather.main.temp_max.ToString(); //Тепмпература в фарингейтах max на сегодня. 
        }

    }
}
