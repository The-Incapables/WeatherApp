using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.DataAccess
{
    public class WeatherDataModel
    {
        [Key]
        public int Id { get; set; }
        public DateTime Datum { get; set; }
        public string Plats { get; set; }
        public double Temp {  get; set; }
        public double Luftfuktighet { get; set; }
    }
}
