using Microsoft.EntityFrameworkCore;
using SafeCity.Domain.Entity;

namespace SafeCity.Domain.Data
{
    public class SafeCityDbContext : DbContext
    {
        public SafeCityDbContext() { }

        public SafeCityDbContext(DbContextOptions<SafeCityDbContext> options)
            : base(options)
        { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<Incident> Incidents { get; set; }
        public virtual DbSet<Case> Cases { get; set; }
        public virtual DbSet<Dispatch> Dispatches { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<Patrol> Patrols { get; set; }
        public virtual DbSet<FieldReport> FieldReports { get; set; }
        public virtual DbSet<Crisis> Crises { get; set; }
        public virtual DbSet<Team> Teams { get; set; }
        public virtual DbSet<Response> Responses { get; set; }
        public virtual DbSet<ComplianceRecord> ComplianceRecords { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=LTIN718874\\SQLEXPRESS;Initial Catalog=SafeCityDB;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False;Command Timeout=30")
                              .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Case>()
                .HasOne(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.AssignedOfficerID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.DispatcherID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Dispatch>()
                .HasOne(d => d.Resource)
                .WithMany()
                .HasForeignKey(d => d.ResourceID)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}