using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }

    [Required]
    [StringLength(255)]
    public string BackgroundColor { get; set; }

    [Required]
    [StringLength(255)]
    public string TextColor { get; set; }

    [InverseProperty("Tag")]
    public virtual ICollection<EventTag> EventTags { get; set; } = new List<EventTag>();
}
