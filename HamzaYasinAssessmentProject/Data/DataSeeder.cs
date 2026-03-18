using HamzaYasinAssessmentProject.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace HamzaYasinAssessmentProject.Server.Data
{
    public static class DataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            context.Database.Migrate();

            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { Id = Guid.NewGuid(), Name = "Alice Johnson", Email = "alice.johnson@email.com", Active = true, Roles = ["Admin", "User"] },
                    new User { Id = Guid.NewGuid(), Name = "Bob Smith", Email = "bob.smith@email.com", Active = true, Roles = ["User"] },
                    new User { Id = Guid.NewGuid(), Name = "Carol White", Email = "carol.white@email.com", Active = true, Roles = ["User", "Moderator"] },
                    new User { Id = Guid.NewGuid(), Name = "David Brown", Email = "david.brown@email.com", Active = false, Roles = ["User"] },
                    new User { Id = Guid.NewGuid(), Name = "Eva Martinez", Email = "eva.martinez@email.com", Active = true, Roles = ["Admin"] },
                    new User { Id = Guid.NewGuid(), Name = "Frank Wilson", Email = "frank.wilson@email.com", Active = true, Roles = ["User"] },
                    new User { Id = Guid.NewGuid(), Name = "Grace Lee", Email = "grace.lee@email.com", Active = false, Roles = ["User", "Moderator"] },
                    new User { Id = Guid.NewGuid(), Name = "Henry Taylor", Email = "henry.taylor@email.com", Active = true, Roles = ["User"] },
                    new User { Id = Guid.NewGuid(), Name = "Isabel Clark", Email = "isabel.clark@email.com", Active = true, Roles = ["Admin", "User"] },
                    new User { Id = Guid.NewGuid(), Name = "James Davis", Email = "james.davis@email.com", Active = false, Roles = ["User"] }
                );

                context.SaveChanges();
            }
        }
    }
}
