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
        public DbSet<QcLab> QcLabs { get; set; }
        public DbSet<ShipmentActivity> ShipmentActivities { get; set; }
        public DbSet<ShipmentEmergency> ShipmentEmergencies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleDriver> VehicleDrivers { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<ProjectPayment> peojectPayments { get; set; }
        public DbSet<VisitLog> VisitLogs { get; set; }
        public DbSet<Sales> Saleses { get; set; }
        public DbSet<SalesCustomer> salesCustomers { get; set; }
    }
}
