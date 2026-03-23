using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SafeCity.Domain.Entity;
using SafeCity.Domain.Enum;

namespace SafeCity.Domain.Data
{
    public class SafeCityDbContext : DbContext
    {
        // private readonly IConfiguration _configuration;
        // public SafeCityDbContext(IConfiguration configuration)
        // {
        //     _configuration = configuration;
        // }

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

        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         optionsBuilder.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"))
        //                       .ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
        //     }
        // }

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

            modelBuilder.Entity<UserRole>().HasData(
                new UserRole { RoleID = 1, RoleName = UserRoleOption.Citizen },
                new UserRole { RoleID = 2, RoleName = UserRoleOption.Police },
                new UserRole { RoleID = 3, RoleName = UserRoleOption.Fire_Fighter },
                new UserRole { RoleID = 4, RoleName = UserRoleOption.Emergency_Dispatcher },
                new UserRole { RoleID = 5, RoleName = UserRoleOption.Compliance_Officer },
                new UserRole { RoleID = 6, RoleName = UserRoleOption.City_Administrator }
            );
        }
    }
}