using System;
using System.IO;
using System.Globalization;
using System.Linq;

namespace WeatherApp.DataAccess
{
    public static class DataLoader
    {
        public static void LoadData(string filepath)
        {
            using (var context = new WeatherContext())
            {
                if (context.WeatherData.Any())
                {
                    return; // Avsluta om det redan finns data
                }

                var lines = File.ReadAllLines(filepath);
                int invalidRowCount = 0; // Räknare för ogiltiga rader

                foreach (var line in lines.Skip(1))
                {
                    var data = line.Split(',');

                    // Validera alla fält
                    if (DateTime.TryParseExact(data[0], "yyyy-MM-dd H:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime datum) &&
                        double.TryParse(data[2], NumberStyles.Any, CultureInfo.InvariantCulture, out double temp) &&
                        double.TryParse(data[3], NumberStyles.Any, CultureInfo.InvariantCulture, out double luftfuktighet))
                    {
                        // Skapa och spara objektet om allt är giltigt
                        var weatherData = new WeatherDataModel
                        {
                            Datum = datum,
                            Plats = data[1],
                            Temp = temp,
                            Luftfuktighet = luftfuktighet
                        };

                        context.WeatherData.Add(weatherData);
                    }
                    else
                    {
                        // Om datan är ogiltig, öka räknaren
                        invalidRowCount++;
                    }
                }

                // Spara alla giltiga rader i databasen
                context.SaveChanges();

                // Skriv ut antalet ogiltiga rader
                Console.WriteLine($"Number of invalid rows skipped: {invalidRowCount}");
            }
        }
    }
}