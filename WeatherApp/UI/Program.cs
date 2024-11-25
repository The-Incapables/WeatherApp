using WeatherApp.DataAccess;
using System;
using System.IO;

namespace WeatherApp.UI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string dbFilePath = Path.Combine(Directory.GetCurrentDirectory(), "WeatherData.db");
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
                    Console.WriteLine("The Database is created!");
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

            //Kontrollera och ladda CSV-filen om den finns
           

            Console.WriteLine("Program finished successfully.");


        }
    }
}