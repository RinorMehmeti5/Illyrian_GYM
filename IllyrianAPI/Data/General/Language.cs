using System;
using System.Collections.Generic;

namespace IllyrianAPI.Data.General;

public partial class Language
{
    public int LanguageId { get; set; }

    public string? NameSq { get; set; }

    public string? NameEn { get; set; }

    public string? Notes { get; set; }

    public virtual ICollection<AspNetUsers> AspNetUsers { get; set; } = new List<AspNetUsers>();
}
