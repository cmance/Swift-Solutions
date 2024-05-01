using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class WeatherForecast
{
    public int Id { get; set; }

    public int WeatherId { get; set; }

    public DateTime Date { get; set; }

    public string Condition { get; set; }

    public double MinTemp { get; set; }

    public double MaxTemp { get; set; }

    public double CloudCover { get; set; }

    public string PrecipitationType { get; set; }

    public double PrecipitationAmount { get; set; }

    public double PrecipitationChance { get; set; }

    public double Humidity { get; set; }

    public virtual Weather Weather { get; set; }
}
