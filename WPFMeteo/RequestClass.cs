using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace WPFMeteo
{
    class RequestClass
    {
        public string Url { get; set; }
        public string NameCity { get; set; }
        public int Id { get; set; }
        public double TempCity { get; set; }
        public string CountryCity { get; set; }

        public RequestClass(string url, string nameCity = "", int id = 0, double tempCity = 0, string countryCity = "")
        {
            this.Url = url;
            this.NameCity = nameCity;
            this.Id = id;
            this.TempCity = tempCity;
            this.CountryCity = countryCity;
        }

        public WebRequest MakeGetRequest(string url, string nameCity = "")
        {
            WebRequest req = WebRequest.Create(url + nameCity);
            return req;
        }

        public string MakeGetResponse(WebRequest req)
        {
            WebResponse resp = req.GetResponse();
            using (Stream stream = resp.GetResponseStream())
            {
                using (StreamReader sr = new StreamReader(stream))
                {
                    string resultGet = sr.ReadToEnd();

                    return resultGet;
                }
            }
        }

        public void MakePostReq(WebRequest request, City city)
        {
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = "POST";
            var jsonData = JsonConvert.SerializeObject(city);
            byte[] byteArray = Encoding.ASCII.GetBytes(jsonData);
            request.ContentType = "application/json";
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
            using (WebResponse response = request.GetResponse())
            {
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    Console.WriteLine(responseFromServer);
                }
            }
        }

        public string GetWeatherCity(WebRequest req)
        {
            req.Method = "POST";
            req.ContentType = "application/x-www-urlencoded";

            using (WebResponse response = req.GetResponse())
            {
                using (Stream s = response.GetResponseStream())
                {
                    using (StreamReader r = new StreamReader(s))
                    {
                        return r.ReadToEnd();
                    }
                }
            }
        }
    }
}
