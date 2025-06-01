using FormsBoard.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace FormsBoard.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<MileageForm> MileageForms { get; set; }
        public DbSet<MileageLineItem> MileageLineItems { get; set; }
        public DbSet<FormStatus> FormStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the relationship between MileageForm and MileageLineItem
            modelBuilder.Entity<MileageLineItem>()
                .HasOne(l => l.MileageForm)
                .WithMany(m => m.LineItems)
                .HasForeignKey(l => l.MileageFormId);

            // Seed FormStatus data
            modelBuilder.Entity<FormStatus>().HasData(
                new FormStatus { Id = 1, Name = "Draft", Description = "Form is being prepared by employee" },
                new FormStatus { Id = 2, Name = "Submitted", Description = "Form submitted for manager review" },
                new FormStatus { Id = 3, Name = "Manager Approved", Description = "Approved by manager, awaiting accounting review" },
                new FormStatus { Id = 4, Name = "Completed", Description = "Accounting approved and completed" },
                new FormStatus { Id = 5, Name = "Rejected", Description = "Rejected for modifications" },
                new FormStatus { Id = 6, Name = "Discarded", Description = "Form has been discarded" }
            );
        }
    }
}
