using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Task
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int UserId { get; set; }

    public bool IsCompleted { get; set; }

    public virtual User User { get; set; } = null!;
}
