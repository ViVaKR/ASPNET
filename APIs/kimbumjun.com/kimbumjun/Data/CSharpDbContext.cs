using kimbumjun.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kimbumjun.Data;

public class CSharpDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public CSharpDbContext() { }

    public CSharpDbContext(DbContextOptions<CSharpDbContext> options) : base(options) { }

    public DbSet<ViV> ViVs { get; set; }
    public DbSet<CSharp> CSharps { get; set; }
    public DbSet<AppUser> AppUsers { get; set; }
    public DbSet<Demo> Demos { get; set; }
}