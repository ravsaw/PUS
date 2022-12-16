using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using PUS.Models;
using System.Reflection.Emit;
using System.Reflection.Metadata;

namespace PUS.Data
{
    public class ApplicationDbContext : IdentityDbContext<Profile, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

		public DbSet<Service> Services { get; set; } = null!;
        public DbSet<Profile> Profiles { get; set; } = null!;
    }
}