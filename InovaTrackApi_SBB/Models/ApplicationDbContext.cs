using Microsoft.EntityFrameworkCore;

namespace InovaTrackApi_SBB.Models

{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BatchingPlant> BatchingPlants { get; set; }
        public DbSet<ProductSlump> ProductSlumps { get; set; }
        public DbSet<ProductGrade> ProductGrades { get; set; }
        public DbSet<ProductStructreTyoe> ProductStructureType { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Geofence> Geofences { get; set; }
        public DbSet<SAPShipment> SAPShipments { get; set; }
        public DbSet<SAPTimeSlot> SAPTimeSlots { get; set; }
        public DbSet<SAPProductMaterial> SAPProductMaterials { get; set; }
        public DbSet<SalesPayment> SalesPayments { get; set; }
        public DbSet<Driver> Drivers { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ProjectsStatus> ProjectStatuses { get; set; }

    }
}
