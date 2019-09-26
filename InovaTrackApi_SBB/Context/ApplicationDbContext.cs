using Microsoft.EntityFrameworkCore;

namespace InovaTrackApi_SBB.Context

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
        public DbSet<ProjectStructureType> ProjectStructureTypes { get; set; }
        public DbSet<Geofence> Geofences { get; set; }
        public DbSet<SAPShipment> SAPShipments { get; set; }
        public DbSet<SAPTimeSlot> SAPTimeSlots { get; set; }
        public DbSet<SAPProductMaterial> SAPProductMaterials { get; set; }
        public DbSet<SalesPayment> SalesPayments { get; set; }
    }
}
