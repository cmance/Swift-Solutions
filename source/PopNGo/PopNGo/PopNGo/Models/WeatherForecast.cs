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

    [Column(TypeName = "float")]
    public double MinTemp { get; set; }

    [Column(TypeName = "float")]
    public double MaxTemp { get; set; }

    [Column(TypeName = "float")]
    public double CloudCover { get; set; }

    [Required]
    [StringLength(255)]
    public string PrecipitationType { get; set; }

    [Column(TypeName = "float")]
    public double PrecipitationAmount { get; set; }

    [Column(TypeName = "float")]
    public double PrecipitationChance { get; set; }

    [Column(TypeName = "float")]
    public double Humidity { get; set; }

    [ForeignKey("WeatherId")]
    [InverseProperty("WeatherForecasts")]
    public virtual Weather Weather { get; set; }
}
