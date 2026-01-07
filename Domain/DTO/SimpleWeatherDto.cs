using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class SimpleWeatherDto
    {
        public double AverageTempC { get; set; }
        public bool WillRain { get; set; }
        public string Message { get; set; }
    }
}
