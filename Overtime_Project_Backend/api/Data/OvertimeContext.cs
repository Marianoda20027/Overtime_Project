using Microsoft.EntityFrameworkCore;
using api.Domain;

namespace Overtime_Project_Backend.Data;

public class OvertimeContext : DbContext
{
    public OvertimeContext(DbContextOptions<OvertimeContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<OvertimeRequest> OvertimeRequests => Set<OvertimeRequest>();
    public DbSet<Approval> OvertimeApprovals => Set<Approval>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Notification> Notifications => Set<Notification>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        // ===== users =====
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
        });

        // ===== overtime_requests =====
        b.Entity<OvertimeRequest>(e =>
        {
            e.ToTable("overtime_requests");
            e.HasKey(x => x.OvertimeId);
            e.Property(x => x.Date).HasColumnType("date");
            e.Property(x => x.StartTime).HasColumnType("time");
            e.Property(x => x.EndTime).HasColumnType("time");
            e.Property(x => x.CostCenter).HasMaxLength(255);
            e.Property(x => x.Justification).HasColumnType("text");
            e.Property(x => x.Status)
             .HasConversion<string>()            // enum -> string
             .HasMaxLength(20)
             .HasDefaultValue(OvertimeStatus.Pending);
            e.Property(x => x.CreatedAt).HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.UpdatedAt).HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.Cost).HasColumnType("decimal(10,2)");

            e.HasOne(x => x.User)
             .WithMany(u => u.OvertimeRequests!)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== overtime_approvals =====
        b.Entity<Approval>(e =>
        {
            e.ToTable("overtime_approvals");
            e.HasKey(x => x.ApprovalId);
            e.Property(x => x.ApprovedHours).HasColumnType("decimal(5,2)");
            e.Property(x => x.ApprovalDate).HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.Status)
             .HasConversion<string>()            // enum -> string
             .HasMaxLength(20)
             .HasDefaultValue(OvertimeStatus.Approved);
            e.Property(x => x.Comments).HasColumnType("text");
            e.Property(x => x.RejectionReason).HasColumnType("text");

            e.HasOne(x => x.Overtime)
             .WithMany(r => r.Approvals!)
             .HasForeignKey(x => x.OvertimeId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Manager)
             .WithMany()
             .HasForeignKey(x => x.ManagerId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ===== roles =====
        b.Entity<Role>(e =>
        {
            e.ToTable("roles");
            e.HasKey(x => x.RoleId);
            e.Property(x => x.RoleName).HasMaxLength(50).IsRequired();
            e.Property(x => x.Permissions).HasColumnType("text");
        });

        // ===== notifications =====
        b.Entity<Notification>(e =>
        {
            e.ToTable("notifications");
            e.HasKey(x => x.NotificationId);
            e.Property(x => x.Message).HasColumnType("text").IsRequired();
            e.Property(x => x.DateSent).HasColumnType("datetime2").HasDefaultValueSql("GETUTCDATE()");
            e.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("sent");

            e.HasOne(x => x.User)
             .WithMany(u => u.Notifications!)
             .HasForeignKey(x => x.UserId)
             .OnDelete(DeleteBehavior.Cascade);
        });
    }

    // Mantener UpdatedAt en UTC
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
