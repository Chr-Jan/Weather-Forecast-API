using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WeatherMap.Models;
using static WeatherMap.Models.WeatherData;

namespace WeatherMap.Controllers
{
    public class WeatherController : Controller
    {
        private readonly string apiKey = "Insert API Key Here";

        private readonly Dictionary<string, string> cities = new Dictionary<string, string>
        {
    {"Nuuk", "3421319"},
    {"Sydney", "2147714"},
    {"Washington DC", "4140963"},
    {"Oymyakon", "2122311"},
    {"Dallol", "2318534"}
        };

        public IActionResult Index()
        {
            var model = new OpenWeatherMap
            {
                Cities = cities
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(OpenWeatherMap model)
        {
            model.Cities = cities;

            try
            {
                string apiUrl;

                if (!string.IsNullOrEmpty(model.SelectedCity))
                {
                    apiUrl = $"http://api.openweathermap.org/data/2.5/weather?id={model.SelectedCity}&appid={apiKey}&units=metric";
                }
                else if (!string.IsNullOrEmpty(model.SearchCity))
                {
                    apiUrl = $"http://api.openweathermap.org/data/2.5/weather?q={model.SearchCity}&appid={apiKey}&units=metric";
                }
                else
                {
                    model.ApiResponse = "► Enter a city name or select one from the list";
                    return View(model);
                }

                using (var httpClient = new HttpClient())
                {
                    var apiResponse = await httpClient.GetStringAsync(apiUrl);
                    var rootObject = JsonConvert.DeserializeObject<ResponseWeather>(apiResponse);

                    model.ApiResponse = $"<table><tr><th>Weather Description</th></tr>" +
                                        $"<tr><td>City:</td><td>{rootObject.name}</td></tr>" +
                                        $"<tr><td>Country:</td><td>{rootObject.sys.country}</td></tr>" +
                                        $"<tr><td>Wind:</td><td>{rootObject.wind.speed} Km/h</td></tr>" +
                                        $"<tr><td>Current Temperature:</td><td>{rootObject.main.temp} °C</td></tr>" +
                                        $"<tr><td>Humidity:</td><td>{rootObject.main.humidity}</td></tr>" +
                                        $"<tr><td>Weather:</td><td>{rootObject.weather[0].description}</td></tr>" +
                                        $"</table>";

                    model.WeatherIconCode = rootObject.weather[0].icon;
                }
            }
            catch (Exception ex)
            {
                model.ApiResponse = $"Error: {ex.Message}";
            }

            return View(model);
        }
    }
}
