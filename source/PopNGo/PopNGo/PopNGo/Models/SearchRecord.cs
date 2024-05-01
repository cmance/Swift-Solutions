using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class SearchRecord
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string SearchQuery { get; set; }

    public DateTime Time { get; set; }
}
