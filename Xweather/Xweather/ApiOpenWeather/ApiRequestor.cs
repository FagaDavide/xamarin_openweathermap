using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Xweather.Models;
using Newtonsoft.Json;

namespace Xweather.ApiOpenWeather
{
    public class ApiRequestor
    {
        private const string URL_BASE = "https://api.openweathermap.org/data/2.5";
        private const string API_KEY_OPENWEATHER = "028fad0a3bdbe8951a4277909e5cf80d";
        //private const string API_KEY_GOOGLE = "AIzaSyDW4d7evFFjY64lK61yiUm2QnIfPp35cMI";
        private const string DEFAULT_CITY = "Neuchatel";
        private const string HEARC_LAT = "46.99734114871151";
        private const string HEARC_LON = "6.938987452848002";
        private const string DEFAULT_LANG = "fr";
        private const string TEMP_UNIT = "metric";
        private const string LIMIT_CNT_MAP = "40";
        private const string LIMIT_CNT_FORCAST = "40";
        private readonly HttpClient httpClient;
     
        public ApiRequestor()
        {
            httpClient = new HttpClient();
        }
        public async Task<ForecastRoot> GetWeatherAreaLatLon(string Lat = HEARC_LAT, string Lon = HEARC_LON)
        {
            var uri = new Uri(URL_BASE + "/find?lat=" + Lat + "&lon=" + Lon +"&cnt=" + LIMIT_CNT_MAP + "&appid=" + API_KEY_OPENWEATHER + "&lang=" + DEFAULT_LANG + "&units=" + TEMP_UNIT);
            string resultInJSON = string.Empty;
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                resultInJSON = await response.Content.ReadAsStringAsync();
            else
                Debug.WriteLine("ApiRequestor - GetForecast - Foireux (city name?)");

            if (string.IsNullOrEmpty(resultInJSON))
                return null;

            //Debug.WriteLine(resultInJSON); //a supprimer en prod

            return JsonConvert.DeserializeObject<ForecastRoot>(resultInJSON);
        }

        public async Task<ForecastRoot> GetWeatherAreaByCity(string City = DEFAULT_CITY)
        {
            var wr = await Task.Run(() => GetCurrentWeather(City));
            var mr = await Task.Run(() => GetWeatherAreaLatLon(wr.coord.lat.ToString(), wr.coord.lon.ToString()));

            return mr;
        }

        public async Task<ForecastRoot> GetForecast(string City = DEFAULT_CITY)
        {
            var uri = new Uri(URL_BASE + "/forecast?q=" + City + "&cnt="+ LIMIT_CNT_FORCAST + "&appid=" + API_KEY_OPENWEATHER + "&lang=" + DEFAULT_LANG + "&units=" + TEMP_UNIT);
            string resultInJSON = string.Empty;
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            resultInJSON = await response.Content.ReadAsStringAsync();
            else
            Debug.WriteLine("ApiRequestor - GetForecast - Foireux (city name?)");

            if (string.IsNullOrEmpty(resultInJSON))
            return null;

            //Debug.WriteLine(resultInJSON); //a supprimer en prod

            return JsonConvert.DeserializeObject<ForecastRoot>(resultInJSON);
        }

        public async Task<ForecastRoot> GetForecastLatLon(string Lat = HEARC_LAT, string Lon = HEARC_LON)
        {
            var uri = new Uri(URL_BASE + "/forecast?lat=" + Lat + "&lon=" + Lon + "&cnt=" + LIMIT_CNT_FORCAST + "&appid=" + API_KEY_OPENWEATHER + "&lang=" + DEFAULT_LANG + "&units=" + TEMP_UNIT);
            string resultInJSON = string.Empty;
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                resultInJSON = await response.Content.ReadAsStringAsync();
            else
                Debug.WriteLine("ApiRequestor - GetForecast - Foireux (city name?)");

            if (string.IsNullOrEmpty(resultInJSON))
                return null;

            //Debug.WriteLine(resultInJSON); //a supprimer en prod

            return JsonConvert.DeserializeObject<ForecastRoot>(resultInJSON);
        }

        public async Task<WeatherRoot> GetCurrentWeather(string City = DEFAULT_CITY)
        {
            var uri = new Uri(URL_BASE + "/weather?q=" + City + "&appid=" + API_KEY_OPENWEATHER + "&lang=" + DEFAULT_LANG + "&units=" + TEMP_UNIT);
            string resultInJSON = string.Empty;
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                resultInJSON = await response.Content.ReadAsStringAsync();
            else
                Debug.WriteLine("ApiRequestor - GetCurrentWeather - Foireux (city name?)");

            if (string.IsNullOrEmpty(resultInJSON))
                return null;

            //Debug.WriteLine(resultInJSON); //a supprimer en prod
        
            return JsonConvert.DeserializeObject<WeatherRoot>(resultInJSON);
        }

        public async Task<WeatherRoot> GetCurrentWeatherLatLon(string Lat = HEARC_LAT, string Lon = HEARC_LON)
        {
            var uri = new Uri(URL_BASE + "/weather?lat=" + Lat + "&lon=" + Lon + "&appid=" + API_KEY_OPENWEATHER + "&lang=" + DEFAULT_LANG + "&units=" + TEMP_UNIT);
            string resultInJSON = string.Empty;
            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                resultInJSON = await response.Content.ReadAsStringAsync();
            else
                Debug.WriteLine("ApiRequestor - GetCurrentWeatherLatLon - Foireux (lat-lon?)");

            if (string.IsNullOrEmpty(resultInJSON))
                return null;

            //Debug.WriteLine(resultInJSON); //a supprimer en prod

            return JsonConvert.DeserializeObject<WeatherRoot>(resultInJSON);
        }
    }
}
