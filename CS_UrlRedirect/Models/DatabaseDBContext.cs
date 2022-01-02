using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace CS_UrlRedirect.Models
{
    public partial class DatabaseDBContext : DbContext
    {
        public DatabaseDBContext()
        {
        }

        public DatabaseDBContext(DbContextOptions<DatabaseDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Redirect> Redirects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\Jr\\source\\repos\\CS_UrlRedirect\\CS_UrlRedirect\\Database\\database.mdf;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Redirect>(entity =>
            {
                entity.Property(e => e.NumVisits).HasColumnName("numVisits");

                entity.Property(e => e.ShortCode)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("shortCode")
                    .IsFixedLength(true);

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("url")
                    .IsFixedLength(true);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
