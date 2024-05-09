using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("Weather")]
public partial class Weather
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal Latitude { get; set; }

    [Column(TypeName = "decimal(9, 6)")]
    public decimal Longitude { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime DateCached { get; set; }

    [InverseProperty("Weather")]
    public virtual ICollection<WeatherForecast> WeatherForecasts { get; set; } = new List<WeatherForecast>();
}
