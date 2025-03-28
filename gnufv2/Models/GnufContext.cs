
// Models/GnufContext.cs
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Models;

public class GnufContext : DbContext
{
    public GnufContext(DbContextOptions<GnufContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
}
