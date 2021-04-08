using System;
using System.Collections.Generic;
using System.Text;

namespace Xweather.Models
{
    /// <summary>
    /// this class is the perfect slipper to consume a json response of a request like this:
    /// https://api.openweathermap.org/data/2.5/weather?q=Neuchatel&appid=028fad0a3bdbe8951a4277909e5cf80d
    /// </summary>
    public class CurrentWeather
    {
        public Coord coord { get; set; }
        public List<Weather> weather { get; set; }
        public string @base { get; set; } //@ because "base" is reserved name
        public Main main { get; set; }
        public int visibility { get; set; }
        public Wind wind { get; set; }
        public Clouds clouds { get; set; }
        public int dt { get; set; }
        public Sys sys { get; set; }
        public int id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
        public String weatherInfoTmp { get; set; }
    }
}
