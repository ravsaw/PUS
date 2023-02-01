using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Hosting;
using PUS.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using static PUS.ViewModels.TransactionListViewModel;

namespace PUS.Data
{
    public class ApplicationDbContext : IdentityDbContext<Profile, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            var connectionStringBuilder = new SqliteConnectionStringBuilder { DataSource = "PusDB.db" };
            var connectionString = connectionStringBuilder.ToString();
            var connection = new SqliteConnection(connectionString);

            optionsBuilder.UseSqlite(connection);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Transaction>()
            .HasOne(t => t.Service)
            .WithMany(t => t.Transactions)
            .OnDelete(DeleteBehavior.ClientSetNull);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder builder)
        {
            builder.Properties<DateOnly>()
                .HaveConversion<DateOnlyConverter>()
                .HaveColumnType("date");
        }
        public DbSet<Service> Services { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatLine> ChatLines { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }

    public class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
    {
        /// <summary>
        /// Creates a new instance of this converter.
        /// </summary>
        public DateOnlyConverter() : base(
                d => d.ToDateTime(TimeOnly.MinValue),
                d => DateOnly.FromDateTime(d))
        { }
    }
}