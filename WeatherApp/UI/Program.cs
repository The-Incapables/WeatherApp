using WeatherApp.DataAccess;
using System;
using System.IO;
using Microsoft.Extensions.Options;
using WeatherApp.Core;
using Microsoft.EntityFrameworkCore;

namespace WeatherApp.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbFilePath = Path.Combine(Directory.GetCurrentDirectory(), "WeatherDataTheIncapables.db");
            string csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "TempFuktData.csv");

            // Kontrollera och skapa databasen om den saknas
            if (File.Exists(dbFilePath))
            {
                Console.WriteLine("Database file found.");
            }
            else
            {
                using (var context = new WeatherContext())
                {
                    context.Database.EnsureCreated(); // Skapa databasen om den inte finns
                    Console.WriteLine("The Database created!");
                    Console.WriteLine("HOLD THE DOOR");
                    Console.WriteLine("HOOLD THE DOOR");
                    Console.WriteLine("HOOOO THE DOOR");
                    Console.WriteLine("HOOOOOOOO DOOOOO");
                    Console.WriteLine("HODOR...");
                }
                if (File.Exists(csvFilePath))
                {
                    DataLoader.LoadData(csvFilePath); // Ladda data från CSV till databasen
                    Console.WriteLine("CSV file loaded into database.");
                }
                else
                {
                    Console.WriteLine("CSV file not found. No data to load.");
                }
            }

            Console.WriteLine("Program loaded successfully.");          

            using (var context = new WeatherContext())
            {
                var weatherCalculation = new WeatherCalculation(context);
                var moldCalculator = new MoldCalculator();
                var seasonCalculator = new SeasonCalculator(weatherCalculation);
                var BkDörr = new VGBalkongDoor();

                // 1. Välj en dag och visa luftfuktighet.
                DateTime specificDate = new DateTime(2016, 10, 22);                 // Exempel, byt datum ifall du vill
                var weatherDataForDate = weatherCalculation.GetWeatherDataByDate(specificDate);
                Console.WriteLine($"Väderdata för {specificDate.ToShortDateString()}:");
                foreach (var data in weatherDataForDate)
                {
                    Console.WriteLine($"Plats: {data.Plats}, Temperatur: {data.Temp}°C, Luftfuktighet: {data.Luftfuktighet}%");
                }

                // 2. Visa medeltemperatur för en specifik dag (inne och ute).
                double avgIndoorTemp = weatherCalculation.AverageTemp(specificDate, "1");
                double avgOutdoorTemp = weatherCalculation.AverageTemp(specificDate, "");
                Console.WriteLine($"\nAverage Temp Indoors: {avgIndoorTemp}°C");
                Console.WriteLine($"Average Temp Outdoors: {avgOutdoorTemp}°C");

                // 3. Beräkna mögelrisken för en specifik dag.
                var indoorHumidity = weatherDataForDate
                    .Where(data => data.Plats == "Inne")
                    .Average(data => data.Luftfuktighet);
                double moldRisk = moldCalculator.CalculateMoldRisk(avgIndoorTemp, indoorHumidity, 1); // Antal dagar = 1
                Console.WriteLine($"\nMold risk for {specificDate.ToShortDateString()}: {moldRisk:F2}");

                // printar 50 första värden i sorterad snitt temp
                weatherCalculation.AverageDayTempSorting();

                // Printar 50 första världen i sorterad snitt fuktighet
                weatherCalculation.AverageFuktSorting();
                Console.WriteLine("\n");
                
                // 4. Identifiera datumen då hösten och vintern börjar.
                var allWeatherData = context.WeatherData.ToList();
                DateTime? autumnStart = seasonCalculator.FindMeteorologicalAutumnDate(allWeatherData);
                DateTime? winterStart = seasonCalculator.FindMeteorologicalWinterDate(allWeatherData);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("Autumn Started:");
                Console.ResetColor(); 
                Console.WriteLine($"{autumnStart?.ToShortDateString() ?? "No data"}");
                Console.ForegroundColor= ConsoleColor.Green;
                Console.Write("Winter Started:");
                Console.ResetColor();
                Console.WriteLine($"{winterStart?.ToShortDateString() ?? "No Meteorological winter has started"}");


                // 5. När Balkongdörren är öppen ( vilket den nästan aldrig är)
                var openTimes = BkDörr.CalculateBalconyDoorOpenTimes(allWeatherData);

                Console.WriteLine("\nTider balkongdörren varit öppen (sorterat):");
                foreach (var (date, openTime) in openTimes)
                {
                    Console.WriteLine($"Datum: {date.ToShortDateString()}, Tid öppen: {openTime} minuter");
                }
            }

            Console.WriteLine("Programmet avslutades framgångsrikt.");
        }
    }
}



