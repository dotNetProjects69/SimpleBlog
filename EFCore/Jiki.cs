using System;
using System.Collections.Generic;

namespace SimpleBlog.EFCore;

public partial class Jiki
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public string? CreatedAt { get; set; }

    public string? UpdatedAt { get; set; }
}
