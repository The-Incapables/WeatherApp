using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.DataAccess;
using static System.Runtime.InteropServices.JavaScript.JSType;

// when the seasons start and end with algorithms for each  

namespace WeatherApp.Core
{
    public class SeasonCalculator
    {
        private readonly WeatherCalculation _weatherCalculation;

        public SeasonCalculator(WeatherCalculation weatherCalculation)
        {
            _weatherCalculation = weatherCalculation;
        }

        public DateTime? FindMeteorologicalAutumnDate
            (List<WeatherDataModel> WeatherData)
        {
            if (WeatherData == null || !WeatherData.Any())
                return null;

            var dbDays = WeatherData.GroupBy(data => data.Datum.Date).ToList();
            var dailyAverageTemps = new List<(DateTime Date, double OutdoorAvg)>();

            foreach (var days in dbDays)
            {
                var outDoorData = days.Where(data => data.Plats == "Ute").ToList();

                var outDoorAvg = outDoorData.Any() ? outDoorData.Average(data => data.Temp) : 0;

                dailyAverageTemps.Add((days.Key, outDoorAvg));
            }

            DateTime? startDate = null;
            int dayTicker = 0;

            foreach (var day in dailyAverageTemps)
            {
                if (day.OutdoorAvg <= 10.0)
                {
                    dayTicker++;
                    if (dayTicker == 5)
                    {
                        startDate = day.Date.AddDays(-4); // Första dagen av 5-dagarsperiod
                        break;
                    }
                }
                else
                {
                    dayTicker = 0; // Återställ om en dag inte uppfyller kravet

                }
            }
            return startDate;
        }


        public DateTime? FindMeteorologicalWinterDate
           (List<WeatherDataModel> WeatherData)
        {
            if (WeatherData == null || !WeatherData.Any())
                return null;

            var dbDays = WeatherData.GroupBy(data => data.Datum.Date).ToList();
            var dailyAverageTemps = new List<(DateTime Date, double OutdoorAvg)>();

            foreach (var days in dbDays)
            {
                var outDoorData = days.Where(data => data.Plats == "Ute").ToList();

                var outDoorAvg = outDoorData.Any() ? outDoorData.Average(data => data.Temp) : 0;

                dailyAverageTemps.Add((days.Key, outDoorAvg));
            }

            DateTime? startDate = null;
            int dayTicker = 0;

            foreach (var day in dailyAverageTemps)
            {
                if (day.OutdoorAvg <= 0.0)
                {
                    dayTicker++;
                    if (dayTicker == 5)
                    {
                        startDate = day.Date.AddDays(-4); // Första dagen av 5-dagarsperiod
                        break;
                    }
                }
                else
                {
                    dayTicker = 0; // Återställ om en dag inte uppfyller kravet

                }
            }
            return startDate;
        }

    }
}
