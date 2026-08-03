using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class News
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public DateOnly Date { get; set; }

    public int AuthorId { get; set; }

    public virtual User Author { get; set; } = null!;
}
