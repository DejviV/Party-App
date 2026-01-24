using Domain.DTO;
using Microsoft.Extensions.Options;
using Service.Interface;
using System.Text.Json;

namespace Service.Implementation
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _http;
        private readonly OpenWeatherOptions _opts;

        public WeatherService(HttpClient http, IOptions<OpenWeatherOptions> opts)
        {
            _http = http;
            _opts = opts.Value;
        }

        public async Task<SimpleWeatherDto> GetSimpleWeatherAsync(string city, DateTime startUtc, DateTime endUtc)
        {
            var res = new SimpleWeatherDto();

                if (string.IsNullOrWhiteSpace(city))
                {
                    res.Message = "City not provided.";
                    return res;
                }

                
                var geoUrl = $"{_opts.GeocodingUrl}?q={Uri.EscapeDataString(city)}&limit=1&appid={_opts.ApiKey}";

                var geoJson = await _http.GetStringAsync(geoUrl);
                using var geoDoc = JsonDocument.Parse(geoJson);

                if (geoDoc.RootElement.ValueKind != JsonValueKind.Array ||
                    geoDoc.RootElement.GetArrayLength() == 0)
                {
                    res.Message = "City not found.";
                    return res;
                }

                var geo = geoDoc.RootElement[0];
                var lat = geo.GetProperty("lat").GetDouble();
                var lon = geo.GetProperty("lon").GetDouble();

                
                var forecastUrl = $"{_opts.OneCallUrl}?lat={lat}&lon={lon}&units=metric&appid={_opts.ApiKey}";

                var forecastJson = await _http.GetStringAsync(forecastUrl);
                using var doc = JsonDocument.Parse(forecastJson);

                if (!doc.RootElement.TryGetProperty("list", out var list) ||
                    list.ValueKind != JsonValueKind.Array)
                {
                    res.Message = "No forecast data available.";
                    return res;
                }

                var temps = new List<double>();
                bool willRain = false;

                foreach (var item in list.EnumerateArray())
                {
                    var dt = item.GetProperty("dt").GetInt64();
                    var tsUtc = DateTimeOffset.FromUnixTimeSeconds(dt).UtcDateTime;

                    if (tsUtc < startUtc || tsUtc > endUtc)
                        continue;

                    
                    if (item.TryGetProperty("main", out var main) &&
                        main.TryGetProperty("temp", out var temp))
                    {
                        temps.Add(temp.GetDouble());
                    }

                    
                    if (item.TryGetProperty("rain", out var rain) &&
                        rain.TryGetProperty("3h", out var rain3h) &&
                        rain3h.GetDouble() > 0)
                    {
                        willRain = true;
                    }

                   
                    if (item.TryGetProperty("weather", out var weatherArr) &&
                        weatherArr.ValueKind == JsonValueKind.Array &&
                        weatherArr.GetArrayLength() > 0)
                    {
                        var mainWeather =
                            weatherArr[0].GetProperty("main").GetString()?.ToLower();

                        if (mainWeather != null &&
                            (mainWeather.Contains("rain") ||
                             mainWeather.Contains("drizzle") ||
                             mainWeather.Contains("thunderstorm")))
                        {
                            willRain = true;
                        }
                    }
                }

                if (!temps.Any())
                {
                    res.Message = "Forecast exists, but no data falls within party time window.";
                    return res;
                }

                res.AverageTempC = temps.Average();
                res.WillRain = willRain;
                res.Message =
                    $"Avg temp: {res.AverageTempC:0.#} °C. Rain expected: {(willRain ? "Yes" : "No")}.";

                return res;
        }
    }
}

