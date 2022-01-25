using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Words.DataAccess;
public class WordsDbContext : DbContext
{
    public virtual DbSet<Word> Words { get; set; }

    public WordsDbContext(DbContextOptions<WordsDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Word>(entity =>
        {
            entity.HasKey(p => p.Id);
        });

        modelBuilder.Seed();
    }
}
