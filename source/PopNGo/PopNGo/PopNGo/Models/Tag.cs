using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

[Table("Tag")]
public partial class Tag
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string BackgroundColor { get; set; }

    public string TextColor { get; set; }
}
