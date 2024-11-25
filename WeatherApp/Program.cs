using WeatherApp.DataAccess;

namespace WeatherApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "TempFuktData.csv");


            using (var context = new WeatherContext())
            {
                context.Database.EnsureCreated();           //Makes sure the Database is created if not already exisiting.
                Console.WriteLine("The Database is created!");
                                                            //Db Filen finns "C:\Users\.....\source\repos\SQL Databas Hantering\WeatherApp\WeatherApp\bin\Debug\net9.0"
            }

            DataLoader.LoadData(filePath);
            Console.WriteLine("Program loaded");

            

        }
    }
}
