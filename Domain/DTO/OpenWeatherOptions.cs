using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class OpenWeatherOptions
    {
        public string ApiKey { get; set; } = "26ebabfe39a790cb9baff9e68255cbfa";
        public string GeocodingUrl { get; set; } = "http://api.openweathermap.org/geo/1.0/direct";
        public string OneCallUrl { get; set; } = "https://api.openweathermap.org/data/2.5/forecast";
    }

}
