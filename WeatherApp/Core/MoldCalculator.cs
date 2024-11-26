using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.DataAccess;

// calculation of the potontiality of mold  

namespace WeatherApp.Core
{
    public class MoldCalculator
    {
        private const double LF_Critical = 75.0;
        private const double K = 0.1;

        public double CalculateMoldRisk(double Temp, double Luftfuktighet, double numberOfDays)
        {
            if (Luftfuktighet > LF_Critical) { return 0; }

            return K * ((Luftfuktighet - LF_Critical) / (100 - LF_Critical)) * numberOfDays;

        }

        public (List<(DateTime Datum, double moldRisk)> Inne, 
            List<(DateTime Datum, double moldRisk)> Ute) CalculateDailyMoldrisk(List<WeatherDataModel> WeatherData)
        {
            var inneMoldRisk = new List<(DateTime Datum, double moldRisk)>();
            var uteMoldRisk = new List<(DateTime Datum, double moldRisk)>();


            var dataExample = WeatherData.GroupBy(data => new { data.Datum, data.Plats }); 

            foreach (var data in dataExample)
            {
                var date = data.Key.Datum;
                var Location = data.Key.Plats;

                var avgTemp = data.Average(data => data.Temp);
                var avgLF = data.Average(data => data.Luftfuktighet);

                var moldRisk = CalculateMoldRisk(avgTemp, avgLF, 1);


                if (Location == "Inne")
                {
                    inneMoldRisk.Add((date, moldRisk));
                } 

                else if (Location == "Ute")
                {
                    uteMoldRisk.Add((date, moldRisk));
                }
            }
            return (inneMoldRisk, uteMoldRisk);
        }

        public void SortMoldRisk(List<WeatherDataModel> WeatherData)
        {
            var (inneMoldRisk, uteMoldRisk) = CalculateDailyMoldrisk(WeatherData);
            var sortedInneMoldRisk = inneMoldRisk.OrderBy(risk => risk.moldRisk).ToList();
            var sortedUteMoldRisk = uteMoldRisk.OrderBy(risk => risk.moldRisk).ToList();

            //Console.WriteLine("Sorted inside mold risk");                     // TESTER!!!!!! :D:D:D:D XD XD XD XP
            //foreach (var (date, moldRisk) in sortedInneMoldRisk)
            //{
            //    Console.WriteLine($"Date: {date.ToShortDateString()}, Mold risk: {moldRisk: F2}");
            //}
            
            //Console.WriteLine("Sorted outside mold risk");
            //foreach (var (date, moldRisk) in sortedUteMoldRisk)
            //{
            //    Console.WriteLine($"Date: {date.ToShortDateString()}, Mold risk: {moldRisk: F2}");
            //}


        }
       
    }
}
