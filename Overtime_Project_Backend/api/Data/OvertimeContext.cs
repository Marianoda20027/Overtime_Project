using Microsoft.EntityFrameworkCore;
using api.Domain;

namespace api.Data
{
    public class OvertimeContext : DbContext
    {
        public OvertimeContext(DbContextOptions<OvertimeContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Manager> Managers => Set<Manager>();
        public DbSet<OvertimeRequest> OvertimeRequests => Set<OvertimeRequest>();
        public DbSet<Approval> OvertimeApprovals => Set<Approval>();
        public DbSet<HumanResource> HumanResources => Set<HumanResource>();

        protected override void OnModelCreating(ModelBuilder b)
        {
            // Managers
            b.Entity<Manager>(e =>
            {
                e.ToTable("managers");
                e.HasKey(x => x.ManagerId);
                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired().HasColumnName("password_hash");
            });

            // Users
            b.Entity<User>(e =>
            {
                e.ToTable("users");
                e.HasKey(x => x.UserId);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired().HasColumnName("password_hash");
                e.Property(x => x.Role).HasMaxLength(50).IsRequired();
                e.Property(x => x.IsActive).HasDefaultValue(true);
                e.Property(x => x.Salary).HasColumnType("decimal(10,2)");
                e.HasOne(x => x.Manager)
                 .WithMany(m => m.Users!)
                 .HasForeignKey(x => x.ManagerId)
                 .OnDelete(DeleteBehavior.Restrict);
            });

            // Overtime Requests
            b.Entity<OvertimeRequest>(e =>
            {
                e.ToTable("overtime_requests");
                e.HasKey(x => x.OvertimeId);
                e.Property(x => x.Date).HasColumnType("date");
                e.Property(x => x.StartTime).HasColumnType("time");
                e.Property(x => x.EndTime).HasColumnType("time");
                e.Property(x => x.Justification).HasColumnType("text");
                e.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasDefaultValue(OvertimeStatus.Pending);
                e.Property(x => x.CreatedAt).HasColumnType("timestamp").HasDefaultValueSql("NOW()");
                e.Property(x => x.UpdatedAt).HasColumnType("timestamp").HasDefaultValueSql("NOW()");
                e.Property(x => x.Cost).HasColumnType("decimal(10,2)");
                e.Property(x => x.TotalHours).HasColumnType("decimal(5,2)").HasDefaultValue(0);

                e.HasOne(x => x.User)
                    .WithMany(u => u.OvertimeRequests!)
                    .HasForeignKey(x => x.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Approvals
            b.Entity<Approval>(e =>
            {
                e.ToTable("overtime_approvals");
                e.HasKey(x => x.ApprovalId);
                e.Property(x => x.ManagerId).HasColumnType("int");
                e.Property(x => x.ApprovedHours).HasColumnType("decimal(5,2)");
                e.Property(x => x.ApprovalDate).HasColumnType("timestamp").HasDefaultValueSql("NOW()");
                e.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .HasDefaultValue(OvertimeStatus.Approved);
                e.Property(x => x.Comments).HasColumnType("text");
                e.Property(x => x.RejectionReason).HasColumnType("text");

                e.HasOne(x => x.Overtime)
                    .WithMany(r => r.Approvals!)
                    .HasForeignKey(x => x.OvertimeId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Human Resources
            b.Entity<HumanResource>(e =>
            {
                e.ToTable("human_resources");
                e.HasKey(x => x.HumanResourceId);
                e.Property(x => x.HumanResourceId).ValueGeneratedOnAdd();
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.Email).HasMaxLength(255).IsRequired();
                e.Property(x => x.PasswordHash).HasMaxLength(255).IsRequired().HasColumnName("password_hash");
            });
        }

        public override int SaveChanges()
        {
            TouchTimestamps();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            TouchTimestamps();
            return base.SaveChangesAsync(ct);
        }

        private void TouchTimestamps()
        {
            var now = DateTime.UtcNow;
            foreach (var e in ChangeTracker.Entries<OvertimeRequest>())
            {
                if (e.State == EntityState.Added)
                {
                    if (e.Entity.CreatedAt == default) e.Entity.CreatedAt = now;
                    e.Entity.UpdatedAt = now;
                }
                else if (e.State == EntityState.Modified)
                {
                    e.Entity.UpdatedAt = now;
                }
            }
        }
    }
}
