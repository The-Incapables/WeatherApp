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

        #region DataMangement
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
        #endregion

        #region TempData
        public double AverageTemp(DateTime Datum, string choice)
        {

            var weatherDataForDate = GetWeatherDataByDate(Datum);
            var indoorData = weatherDataForDate.Where(data => data.Plats == "Inne").ToList();
            var outdoorData = weatherDataForDate.Where(data => data.Plats == "Ute").ToList();

            var averageIndoorTemp = indoorData.Any() ? indoorData.Average(data => data.Temp) : 0;
            var averageOutdoorTemp = indoorData.Any() ? outdoorData.Average(data => data.Temp) : 0;

            if (choice == "1")
            {
                return (averageIndoorTemp);
            }
            else
            { 
                return (averageOutdoorTemp);
            }
            
        }
        #endregion

        #region SortingData
        public void AverageDayTempSorting()            // Soreterar avg temp för hela CSV
        {
            
            var allDates = _context.WeatherData.Select(data => data.Datum.Date).Distinct().ToList();        // Hämtar alla datum

            
            var dailyAverageTemps = new List<(DateTime Date, double IndoorAvg, double OutdoorAvg)>();       // Räknar ut avg temp/dag

            foreach (var date in allDates)
            {
                var weatherDataForDate = GetWeatherDataByDate(date);

                
                var indoorData = weatherDataForDate.Where(data => data.Plats == "Inne").ToList();       // Separerar inne & ute
                var outdoorData = weatherDataForDate.Where(data => data.Plats == "Ute").ToList();

                
                var indoorAvg = indoorData.Any() ? indoorData.Average(data => data.Temp) : 0;
                var outdoorAvg = outdoorData.Any() ? outdoorData.Average(data => data.Temp) : 0;        // räknar ut avg temp 

                dailyAverageTemps.Add((date, indoorAvg, outdoorAvg));
            }

            
            var sortedIndoorTemps = dailyAverageTemps
                .OrderByDescending(day => day.IndoorAvg)
                .ToList();
                                                                    // Sorterar för inne och ute
            var sortedOutdoorTemps = dailyAverageTemps
                .OrderByDescending(day => day.OutdoorAvg)
                .ToList();



            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nIndoor Temperatures (Warmest to Coldest): Date, Indoor Temp, OutDoor Temp");       
            Console.ResetColor();
            for (int i = 0;  i < 50; i++)
            {
                Console.WriteLine(sortedIndoorTemps[i]);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nOutdoor Temperatures (Warmest to Coldest):Date, Indoor Temp, OutDoor Temp");       
            Console.ResetColor();
            for (int i = 0;  i < 50; i++)
            {
                Console.WriteLine(sortedOutdoorTemps[i]);
            }

            
        }
        
        public void AverageFuktSorting()
        {
            var allDates = _context.WeatherData.Select(data => data.Datum.Date).Distinct().ToList();

            var dailyAverageFukt = new List<(DateTime Date, double IndoorAvg, double OutdoorAvg)>();

            foreach (var date in allDates)
            {
                var weatherDataForDate = GetWeatherDataByDate(date);


                var indoorData = weatherDataForDate.Where(data => data.Plats == "Inne").ToList();       // Separerar inne & ute
                var outdoorData = weatherDataForDate.Where(data => data.Plats == "Ute").ToList();


                var indoorAvg = indoorData.Any() ? indoorData.Average(data => data.Luftfuktighet) : 0;
                var outdoorAvg = outdoorData.Any() ? outdoorData.Average(data => data.Luftfuktighet) : 0;        // räknar ut avg temp 

                dailyAverageFukt.Add((date, indoorAvg, outdoorAvg));
            }
            var sortedIndoorFukt = dailyAverageFukt
                .OrderBy(day => day.IndoorAvg) 
                .ToList();
            // Sorterar för inne och ute
            var sortedOutdoorFukt = dailyAverageFukt
                .OrderBy(day => day.OutdoorAvg)
                .ToList();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nIndoor Humidity (Driest to most Humid): Date, Indoor Humidity, OutDoor Humidity");
            Console.ResetColor();
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(sortedIndoorFukt[i]);
            }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n\nOutdoor Humidity (Driest to most Humid): Date, Indoor Humidity, OutDoor Humidity");
            Console.ResetColor();
            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine(sortedOutdoorFukt[i]);
            }
            
        }
        #endregion
    }
}
