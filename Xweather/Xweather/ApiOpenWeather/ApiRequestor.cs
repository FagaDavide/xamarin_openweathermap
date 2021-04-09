using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xweather.Models;
using Newtonsoft.Json;

namespace Xweather.ApiOpenWeather
{
    class ApiRequestor
    {
        const string URL_BASE = "https://api.openweathermap.org/data/2.5";
        const string API_KEY = "028fad0a3bdbe8951a4277909e5cf80d";
        const string DEFAULT_CITY = "Neuchatel";
        const string DEFAULT_LANG = "fr";
        const string TEMP_UNIT = "metric";
        readonly HttpClient httpClient;
     
        public ApiRequestor()
        {
            httpClient = new HttpClient();
        }


        //ResponseCurrentMeteo rcm = JsonConvert.DeserializeObject<ResponseCurrentMeteo>(result);
        public async Task<WeatherRoot> GetCurrentWeather(String City = DEFAULT_CITY)
        {
            Uri uri = new Uri(URL_BASE + "/weather?q=" + City + "&appid=" + API_KEY + "&lang=" + DEFAULT_LANG + "&units=" + TEMP_UNIT);
            var resultInJSON = string.Empty;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                resultInJSON = await response.Content.ReadAsStringAsync();
            else
                Debug.WriteLine("ApiRequestor - GetCurrentWeather - Foireux (city name?)");

            if (string.IsNullOrEmpty(resultInJSON))
                return null;

            Debug.WriteLine(resultInJSON); //a supprimer en prod
        
            return JsonConvert.DeserializeObject<WeatherRoot>(resultInJSON);
        }
    }
}
