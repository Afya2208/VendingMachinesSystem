using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Note
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string Text { get; set; } = null!;

    public int UserId { get; set; }

    public virtual User User { get; set; } = null!;
}
