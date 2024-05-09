using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;

[Table("WeatherForecast")]
public partial class WeatherForecast
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int WeatherId { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Date { get; set; }

    [Required]
    [StringLength(255)]
    public string Condition { get; set; }

    public double MinTemp { get; set; }

    public double MaxTemp { get; set; }

    public double CloudCover { get; set; }

    [Required]
    [StringLength(255)]
    public string PrecipitationType { get; set; }

    public double PrecipitationAmount { get; set; }

    public double PrecipitationChance { get; set; }

    public double Humidity { get; set; }

    [ForeignKey("WeatherId")]
    [InverseProperty("WeatherForecasts")]
    public virtual Weather Weather { get; set; }
}
