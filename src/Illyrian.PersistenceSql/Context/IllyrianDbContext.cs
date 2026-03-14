using Illyrian.Domain.Entities;
using Illyrian.Domain.Repositories;
using Illyrian.Persistence.General;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Illyrian.PersistenceSql.Context;

public class IllyrianDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IApplicationDbContext
{
    public IllyrianDbContext(DbContextOptions<IllyrianDbContext> options) : base(options) { }

    public DbSet<Membership> Memberships { get; set; } = null!;
    public DbSet<MembershipType> MembershipTypes { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<GymClass> GymClasses { get; set; } = null!;
    public DbSet<Exercise> Exercises { get; set; } = null!;
    public DbSet<Schedule> Schedules { get; set; } = null!;
    public DbSet<UserClass> UserClasses { get; set; } = null!;
    public DbSet<UserSchedule> UserSchedules { get; set; } = null!;
    public DbSet<Language> Languages { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<RoleMenu> RoleMenus { get; set; } = null!;
    public DbSet<StatusType> StatusTypes { get; set; } = null!;
    public DbSet<LogEntry> Logs { get; set; } = null!;
    public DbSet<TestItem> TestItems { get; set; } = null!;

    // IApplicationDbContext explicit implementations
    IQueryable<Membership> IApplicationDbContext.Memberships => Memberships;
    IQueryable<MembershipType> IApplicationDbContext.MembershipTypes => MembershipTypes;
    IQueryable<Payment> IApplicationDbContext.Payments => Payments;
    IQueryable<GymClass> IApplicationDbContext.GymClasses => GymClasses;
    IQueryable<Exercise> IApplicationDbContext.Exercises => Exercises;
    IQueryable<Schedule> IApplicationDbContext.Schedules => Schedules;
    IQueryable<UserClass> IApplicationDbContext.UserClasses => UserClasses;
    IQueryable<UserSchedule> IApplicationDbContext.UserSchedules => UserSchedules;
    IQueryable<Language> IApplicationDbContext.Languages => Languages;
    IQueryable<Menu> IApplicationDbContext.Menus => Menus;
    IQueryable<RoleMenu> IApplicationDbContext.RoleMenus => RoleMenus;
    IQueryable<StatusType> IApplicationDbContext.StatusTypes => StatusTypes;
    IQueryable<LogEntry> IApplicationDbContext.Logs => Logs;
    IQueryable<TestItem> IApplicationDbContext.TestItems => TestItems;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(IllyrianDbContext).Assembly);

        // Configure ApplicationUser extra columns matching existing database
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.CityID).HasColumnName("CityID");
            entity.Property(e => e.SettlementID).HasColumnName("SettlementID");
            entity.Property(e => e.LanguageID).HasColumnName("LanguageID");

            entity.HasOne<Language>()
                .WithMany()
                .HasForeignKey(e => e.LanguageID)
                .HasConstraintName("FK_AspNetUsers_Language");
        });

        // Configure ApplicationRole extra columns
        modelBuilder.Entity<ApplicationRole>(entity =>
        {
            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name_EN).HasMaxLength(128).HasColumnName("Name_EN");
            entity.Property(e => e.Name_SQ).HasMaxLength(128).HasColumnName("Name_SQ");
        });

        // Ignore domain User/Role navigation properties that don't map to EF
        modelBuilder.Entity<Membership>().Ignore(e => e.User);
        modelBuilder.Entity<Payment>().Ignore(e => e.User);
        modelBuilder.Entity<UserClass>().Ignore(e => e.User);
        modelBuilder.Entity<UserSchedule>().Ignore(e => e.User);
        modelBuilder.Entity<LogEntry>().Ignore(e => e.User);
        modelBuilder.Entity<Menu>().Ignore(e => e.CreatedByNavigation);
        modelBuilder.Entity<Menu>().Ignore(e => e.ModifiedByNavigation);
        modelBuilder.Entity<RoleMenu>().Ignore(e => e.CreatedByNavigation);
        modelBuilder.Entity<RoleMenu>().Ignore(e => e.ModifiedByNavigation);
        modelBuilder.Entity<RoleMenu>().Ignore(e => e.Role);
        modelBuilder.Entity<StatusType>().Ignore(e => e.InsertedFromNavigation);
        modelBuilder.Entity<StatusType>().Ignore(e => e.UpdatedFromNavigation);
    }
}
