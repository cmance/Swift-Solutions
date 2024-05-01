using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class TicketLink
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public string Source { get; set; }

    public string Link { get; set; }

    public virtual Event Event { get; set; }
}
