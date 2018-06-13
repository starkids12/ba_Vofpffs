﻿using Microsoft.EntityFrameworkCore;

namespace ba_Vofpffs.Models
{
    public class FileEntryContext : DbContext
    {
        public FileEntryContext(DbContextOptions<FileEntryContext> options) : base (options) { }

        public DbSet<FileEntryItemA> FileEntryItemsA { get; set; }
        public DbSet<FileEntryItemB> FileEntryItemsB { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileEntryItemA> ().ToTable ("FileEntryA");
            modelBuilder.Entity<FileEntryItemB> ().ToTable ("FileEntryB");
        }
    }
}
