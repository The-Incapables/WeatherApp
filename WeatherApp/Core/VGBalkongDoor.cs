using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.DataAccess;

namespace WeatherApp.Core
{
    public class VGBalkongDoor
    {


        public List<(DateTime Date, int OpenTimeMinutes)> CalculateBalconyDoorOpenTimes(List<WeatherDataModel> weatherData)
        {
            
            var openTimes = new List<(DateTime Date, int OpenTimeMinutes)>();       // Lista för att spara datum och öppettider

            
            var groupedByDate = weatherData.GroupBy(w => w.Datum.Date);             // Gruppera data per dag

            foreach (var dayGroup in groupedByDate)
            {
                int openMinutes = 0;

                
                var indoorData = dayGroup.Where(w => w.Plats == "Inne").OrderBy(w => w.Datum).ToList();     // Separera data för inomhus och utomhus
                var outdoorData = dayGroup.Where(w => w.Plats == "Ute").OrderBy(w => w.Datum).ToList();

                
                for (int i = 1; i < indoorData.Count; i++)          // Iterera genom inomhusdata
                {
                    var prevIndoor = indoorData[i - 1];
                    var currIndoor = indoorData[i];
                    
                    double indoorTempDrop = prevIndoor.Temp - currIndoor.Temp;          // Kontrollera om innetemperaturen sjunker

                    if (indoorTempDrop > 0.5) // Tröskelvärde för innetemperaturens sänkning
                    {

                        var matchingOutdoorPrev = outdoorData.FirstOrDefault(o => o.Datum == prevIndoor.Datum);
                        var matchingOutdoorCurr = outdoorData.FirstOrDefault(o => o.Datum == currIndoor.Datum);

                        if (matchingOutdoorPrev != null && matchingOutdoorCurr != null)
                        {
                            
                            double outdoorTempRise = matchingOutdoorCurr.Temp - matchingOutdoorPrev.Temp;       // Kontrollera om utetemperaturen stiger

                            if (outdoorTempRise > 0.2) // Tröskelvärde för utetemperaturens ökning
                            {
                                
                                openMinutes++;              // Om både innetemperaturen sjunker och utetemperaturen stiger,
                                                            // anta att dörren var öppen för denna tidsintervall
                            }
                        }
                    }
                }

                
                openTimes.Add((dayGroup.Key, openMinutes));         // Lägg till resultatet för denna dag
            }

            //var daysWithOpenTime = openTimes.Where(t => t.OpenTimeMinutes > 1).ToList();          //Va tungen att kolla ifall det fanns någon dag....

            //if (daysWithOpenTime.Any())
            //{
            //    Console.WriteLine("\nDagar där balkongdörren varit öppen mer än 1 minut:");
            //    foreach (var (date, openTime) in daysWithOpenTime)
            //    {
            //        Console.WriteLine($"Datum: {date.ToShortDateString()}, Tid öppen: {openTime} minuter");
            //    }
            //}
            //else
            //{
            //    Console.WriteLine("\nDet finns inga dagar där balkongdörren varit öppen mer än 1 minut.");
            //}

            return openTimes.OrderByDescending(t => t.OpenTimeMinutes).ToList();            // Sortera resultatet baserat på tid som dörren var öppen
        }
    }
}
