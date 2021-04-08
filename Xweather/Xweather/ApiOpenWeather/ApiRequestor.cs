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
        HttpClient httpClient;
     
        public ApiRequestor()
        {
            httpClient = new HttpClient();
        }


        //ResponseCurrentMeteo rcm = JsonConvert.DeserializeObject<ResponseCurrentMeteo>(result);
        public async Task<String> getCurrentWeather(String City = DEFAULT_CITY)
        {
            Uri uri = new Uri(URL_BASE + "/weather?q=" + City + "&appid=" + API_KEY);
            var resultInJSON = string.Empty;
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                resultInJSON = await response.Content.ReadAsStringAsync();
            else
                Debug.WriteLine("Foireux");

            if (string.IsNullOrWhiteSpace(resultInJSON))
                return null;

            CurrentWeather toto = JsonConvert.DeserializeObject<CurrentWeather>(resultInJSON);
            Debug.WriteLine("OK1 - result");
            Debug.WriteLine(resultInJSON);
            Debug.WriteLine("OK2");
            Debug.WriteLine("OK3 - json");
            Debug.WriteLine(toto);
            Debug.WriteLine("OK4");
            Debug.WriteLine("OK5 - test");
            Debug.WriteLine(toto.visibility);
            Debug.WriteLine(toto.coord);





            return resultInJSON;
        }
    }
}
