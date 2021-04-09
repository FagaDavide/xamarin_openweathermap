using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Xweather.Models
{
    /// <summary>
    /// this class is the perfect slipper to consume a json response of a request like this:
    /// https://api.openweathermap.org/data/2.5/weather?q=Neuchatel&appid=028fad0a3bdbe8951a4277909e5cf80d
    /// and is made with https://json2csharp.com/
    /// </summary>
    /// 
    // Root myDeserializedClass = JsonConvert.DeserializeObject<WeatherRoot>(myJsonResponse); 
    public class Coord
    {
        public double lon { get; set; } = 0.0;
        public double lat { get; set; } = 0.0;
    }

    public class Weather
    {
        public int id { get; set; } = 0;
        public string main { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string icon { get; set; } = string.Empty;

        [JsonIgnore]
        public string GetIcon {
            get { return $"https://openweathermap.org/img/w/{icon}.png"; }
        }
    }

    public class Main
    {
        public double temp { get; set; } = 0.0;
        public double feels_like { get; set; } = 0.0;
        public double temp_min { get; set; } = 0.0;
        public double temp_max { get; set; } = 0.0;
        public int pressure { get; set; } = 0;
        public int humidity { get; set; } = 0;
    }

    public class Wind
    {
        public double speed { get; set; } = 0.0;
        public int deg { get; set; } = 0;
    }

    public class Clouds
    {
        public int all { get; set; } = 0;
    }

    public class Sys
    {
        public int type { get; set; } = 0;
        public int id { get; set; } = 0;
        public string country { get; set; } = string.Empty;
        public int sunrise { get; set; } = 0;
        public int sunset { get; set; } = 0;
    }

    public class WeatherRoot
    {
        public Coord coord { get; set; } = new Coord();
        public List<Weather> weather { get; set; } = new List<Weather>();
        public string @base { get; set; } = string.Empty;
        public Main main { get; set; } = new Main();
        public int visibility { get; set; } = 0;
        public Wind wind { get; set; } = new Wind();
        public Clouds clouds { get; set; } = new Clouds();
        public int dt { get; set; } = 0;
        public Sys sys { get; set; } = new Sys();
        public int timezone { get; set; } = 0;
        public int id { get; set; } = 0;
        public string name { get; set; } = string.Empty;
        public int cod { get; set; } = 0;
    }
}
