using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace PopNGo.Models;

[Table("EventHistory")]
public partial class EventHistory
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int EventId { get; set; }

    public DateTime ViewedDate { get; set; }

    public virtual Event Event { get; set; }

    public virtual PgUser User { get; set; }
}
