using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Logs
{
    public long Id { get; set; }

    public string UserId { get; set; } = null!;

    public string Ip { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string HttpMethod { get; set; } = null!;

    public string Controller { get; set; } = null!;

    public string Action { get; set; } = null!;

    public bool Error { get; set; }

    public string? FormContent { get; set; }

    public string? Response { get; set; }

    public string? Exception { get; set; }

    public DateTime InsertedDate { get; set; }

    public virtual AspNetUsers User { get; set; } = null!;
}
