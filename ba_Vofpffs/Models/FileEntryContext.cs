using Microsoft.EntityFrameworkCore;

namespace ba_Vofpffs.Models
{
    public class FileEntryContext : DbContext
    {
        public FileEntryContext(DbContextOptions<FileEntryContext> options) : base (options) { }

        public DbSet<FileEntryItem> FileEntryItems { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite ("Data Source=FileEntrys.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileEntryItem> ().ToTable ("FileEntry");
        }
    }
}
