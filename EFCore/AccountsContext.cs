using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SimpleBlog.Models.Account;
using SimpleBlog.Models.Authentication;
using static SimpleBlog.Shared.GlobalParams;

namespace SimpleBlog.EFCore;

public partial class AccountsContext : DbContext
{
    DbContextOptions<AccountsContext> _options;

    public AccountsContext()
    {
        Database.EnsureCreated();
    }

    public AccountsContext(DbContextOptions<AccountsContext> options)
        : base(options)
    {
        _options = options;
    }

    public virtual DbSet<AccountInfoModel> Users { get; set; } = null!;

    public virtual DbSet<SignUpModel> AuthData { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlite(GetAccountsDataPath());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
