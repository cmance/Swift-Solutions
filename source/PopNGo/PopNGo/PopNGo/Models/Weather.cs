using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class Weather
{
    public int Id { get; set; }

    public decimal Latitude { get; set; }

    public decimal Longitude { get; set; }

    public DateTime DateCached { get; set; }

    public virtual ICollection<WeatherForecast> WeatherForecasts { get; set; } = new List<WeatherForecast>();
}
