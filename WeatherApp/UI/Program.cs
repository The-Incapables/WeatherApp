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
            }



            Console.WriteLine("Program finished successfully.");


        }
    }
}