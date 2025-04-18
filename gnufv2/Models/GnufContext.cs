using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gnuf.Models
{
    public class GnufContext : DbContext
    {
        public GnufContext(DbContextOptions<GnufContext> options) : base(options) { }

        public DbSet<UserStructure> Users { get; set; } = null!;
        public DbSet<PostStructure> Post { get; set; } = null!;
        public DbSet<CommunityStructure> Community { get; set; } = null!;
        public DbSet<FeedbackStructure> Feedback { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define a Unix timestamp <-> DateTime converter
            var dateTimeConverter = new ValueConverter<DateTime, long>(
                v => new DateTimeOffset(v).ToUnixTimeSeconds(),
                v => DateTimeOffset.FromUnixTimeSeconds(v).UtcDateTime
            );

            // Apply it to the Timestamp property in PostStructure
            modelBuilder.Entity<PostStructure>()
                .Property(p => p.timestamp)
                .HasConversion(dateTimeConverter);

            base.OnModelCreating(modelBuilder);
        }
    }
}
