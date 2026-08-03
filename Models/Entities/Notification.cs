using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public string Description { get; set; } = null!;

    public string Title { get; set; } = null!;
}
