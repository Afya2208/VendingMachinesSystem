using System;
using System.Collections.Generic;

namespace Models.Entities;

public partial class Report
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public DateTime? DateTimePublished { get; set; }

    public int ReportTypeId { get; set; }

    public int ReportStatusId { get; set; }

    public string? Description { get; set; }

    public string? MeterReadings { get; set; }

    public virtual ReportStatus ReportStatus { get; set; } = null!;

    public virtual ReportType ReportType { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
