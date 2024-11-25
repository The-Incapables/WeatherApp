using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherApp.DataAccess
{
    public class WeatherContext : DbContext
    {
        public DbSet<WeatherDataModel> WeatherData { get; set; } 

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=WeatherData.db");
            //Db Filen finns "C:\Users\.....\source\repos\SQL Databas Hantering\WeatherApp\WeatherApp\bin\Debug\net9.0"
        }


    }
}
