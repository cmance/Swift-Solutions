﻿using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class ScheduledNotification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime Time { get; set; }

    public string Type { get; set; }

    public virtual PgUser User { get; set; }
}
