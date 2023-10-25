using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CRUD.Models;

namespace CRUD.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<GladiatorModel> GladiatorModel { get; set; } = default!;
        //public DbSet<UserModel> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            
            // Configure the one-to-many relationship
            modelBuilder.Entity<UserModel>()
                .HasMany(user => user.gladiators)
                .WithOne(gladiator => gladiator.User)
                .HasForeignKey(gladiator => gladiator.user_id);
            
        }
    }
}