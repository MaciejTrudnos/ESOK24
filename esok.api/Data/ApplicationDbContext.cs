using esok.api.Application.Common.Enum;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace esok.api.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, int>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder
                .Entity<ApplicationRole>()
                .HasData(
                    new ApplicationRole() { Id = (int) Role.Manager, Name = Role.Manager.ToString(), NormalizedName = Role.Manager.ToString().ToUpper(), ConcurrencyStamp = "9b6ef024-c4ef-40b5-ac2b-8ad15e20a0c1" },
                    new ApplicationRole() { Id = (int) Role.Employee, Name = Role.Employee.ToString(), NormalizedName = Role.Employee.ToString().ToUpper(), ConcurrencyStamp = "50dd45b0-c4b5-42cd-9556-f1afa047da76" });

            modelBuilder
                .Entity<ApplicationUser>()
                .Property(c => c.Active)
                .HasDefaultValue(true);
        }

        public DbSet<Group> Group { get; set; }
        public DbSet<UserGroup> UserGroup { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Rent> Rent { get; set; }
        public DbSet<RentProduct> RentProduct { get; set; }
        public DbSet<RentFinished> RentFinished { get; set; }
        public DbSet<ChatbotQuestionAnswer> ChatbotQuestionAnswer { get; set; }
        public DbSet<Article> Article { get; set; }
    }
}
