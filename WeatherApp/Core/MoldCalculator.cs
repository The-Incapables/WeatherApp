using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// calculation of the potontiality of mold  

namespace WeatherApp.Core
{
    public static class MoldCalculator
    {

        public static double CalcullateMoldRisk(double Temp, double Luftfuktighet)
        {
            return (Temp + Luftfuktighet) / 2; 
        }
    }
}
