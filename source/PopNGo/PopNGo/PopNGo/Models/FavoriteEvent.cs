using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace PopNGo.Models;


[Table("FavoriteEvents")]
public class FavoriteEvents
{
    [Key]
    [Column("ID")]
    public int ID { get; set; }
    [Column("UserID")]
    public int UserID { get; set; }
    [Column("EventID")]
    public string EventID { get; set; }

    // Navigation properties

    [ForeignKey("UserID")] // 
    public virtual PgUser User { get; set; }
    [ForeignKey("EventID")] //
    public virtual Event Event { get; set; }
}