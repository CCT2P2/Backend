
// Models/GnufContext.cs
using Microsoft.EntityFrameworkCore;

namespace Gnuf.Models;

public class GnufContext : DbContext
{
    public GnufContext(DbContextOptions<GnufContext> options) : base(options) { }

    public DbSet<UserStructure> Users { get; set; } = null!;
    public DbSet<PostStructure> Post { get; set; } = null!;
    public DbSet<CommunityStructure> Community { get; set; } = null!;
    public DbSet<FeedbackStructure> Feedback { get; set; } = null!;
}
