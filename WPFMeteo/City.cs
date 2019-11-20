using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFMeteo
{
    public class City
    {
        public int Id { get; set; }
        public string NameCity { get; set; }
        public double Temperature { get; set; }
        public string Country { get; set; }

        public City(int id, string nameCity, double temperature, string country)
        {
            this.Id = id;
            this.NameCity = nameCity;
            this.Temperature = temperature;
            this.Country = country;
        }
    }
}
