namespace WeatherMap.Models
{
    public class OpenWeatherMap
    {
        public string ApiResponse { get; set; }
        public Dictionary<string, string> Cities { get; set; }
        public string SelectedCity { get; set; }
        public string SearchCity { get; set; }

        public string WeatherIconCode { get; set; }
    }
}
