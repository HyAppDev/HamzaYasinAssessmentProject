using HamzaYasinAssessmentProject.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HamzaYasinAssessmentProject.Server.Data
{
    public class AppDbContext : DbContext
    {
       

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }


        //Need to accomodate the List<string> Roles property in the User model,
        //we will use a value converter to store it as a comma-separated string in the database.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.Roles)
                .HasConversion(
                    v => string.Join(',', v),
                    v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()
                );
        }

        public DbSet<User> Users { get; set; }

    }
}
