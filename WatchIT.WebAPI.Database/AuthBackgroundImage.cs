using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class AuthBackgroundImage
{
    public short Id { get; set; }

    public string? Description { get; set; }

    public string ContentType { get; set; } = null!;

    public byte[] Image { get; set; } = null!;
}
