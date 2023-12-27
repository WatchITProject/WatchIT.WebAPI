using System;
using System.Collections.Generic;

namespace WatchIT.WebAPI.Database;

public partial class AccountRefreshToken
{
    public Guid Id { get; set; }

    public int AccountId { get; set; }

    public DateTime ExpirationDate { get; set; }

    public virtual Account Account { get; set; } = null!;
}
