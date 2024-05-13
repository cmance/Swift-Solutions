using System;
using System.Collections.Generic;

namespace PopNGo.Models;

public partial class AccountRecord
{
    public int Id { get; set; }

    public DateTime Day { get; set; }

    public int AccountsCreated { get; set; }

    public int AccountsDeleted { get; set; }
}
