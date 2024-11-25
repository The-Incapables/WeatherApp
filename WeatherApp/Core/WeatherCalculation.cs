using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.DataAccess;

// average max and min for day or week or month or year  

namespace WeatherApp.Core
{
    public class WeatherCalculation
    {
        private readonly WeatherContext _context;
        public WeatherCalculation(WeatherContext context)
        {
            _context = context;
        }

        #region DataManagement
        public void AddWeatherData(DateTime Datum, string Plats, double Temp, double LuftFuktighet)
        {
            var newWeatherData = new WeatherDataModel
            {
                Datum = Datum,
                Plats = Plats,
                Temp = Temp,
                Luftfuktighet = LuftFuktighet
            };
            _context.WeatherData.Add(newWeatherData);
            _context.SaveChanges();
        }
        

        public List<WeatherDataModel> GetWeatherDataByDate(DateTime Datum)
        {
            return _context.WeatherData.Where(data => data.Datum == Datum).ToList();
        }


        public (double inDoorAverage, double outDoorAverage) AverageTemp(DateTime Datum)
        {
            var weatherDataForDate = GetWeatherDataByDate(Datum);
            var indoorData = weatherDataForDate.Where(data => data.Plats == "Inne").ToList();
            var outdoorData = weatherDataForDate.Where(data => data.Plats == "Ute").ToList();

            var averageIndoorTemp = indoorData.Any() ? indoorData.Average(data => data.Temp) : 0;
            var averageOutdoorTemp = indoorData.Any() ? outdoorData.Average(data => data.Temp) : 0;


            return (averageIndoorTemp, averageOutdoorTemp);
        }

        public void averageDayTempSortingAlgorithm()
        {

        }


        #endregion
    }
}
