using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace IllyrianAPI.Data.General;

public partial class IllyrianContext : DbContext
{
    public IllyrianContext()
    {
    }

    public IllyrianContext(DbContextOptions<IllyrianContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }

    public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }

    public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }

    public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }

    public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }

    public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

    public virtual DbSet<Classes> Classes { get; set; }

    public virtual DbSet<Exercises> Exercises { get; set; }

    public virtual DbSet<Language> Language { get; set; }

    public virtual DbSet<Logs> Logs { get; set; }

    public virtual DbSet<MembershipTypes> MembershipTypes { get; set; }

    public virtual DbSet<Memberships> Memberships { get; set; }

    public virtual DbSet<Menu> Menu { get; set; }

    public virtual DbSet<Payments> Payments { get; set; }

    public virtual DbSet<RoleMenus> RoleMenus { get; set; }

    public virtual DbSet<Schedule> Schedule { get; set; }

    public virtual DbSet<StatusType> StatusType { get; set; }

    public virtual DbSet<Test> Test { get; set; }

    public virtual DbSet<UserClasses> UserClasses { get; set; }

    public virtual DbSet<UsersSchedule> UsersSchedule { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost;Database=Illyrian;Trusted_Connection=True;TrustServerCertificate=True", x => x.UseNetTopologySuite());

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AspNetRoleClaims>(entity =>
        {
            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.HasOne(d => d.Role).WithMany(p => p.AspNetRoleClaims).HasForeignKey(d => d.RoleId);
        });

        modelBuilder.Entity<AspNetRoles>(entity =>
        {
            entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedName] IS NOT NULL)");

            entity.Property(e => e.Description).HasMaxLength(4000);
            entity.Property(e => e.Name).HasMaxLength(256);
            entity.Property(e => e.NameEn)
                .HasMaxLength(128)
                .HasColumnName("Name_EN");
            entity.Property(e => e.NameSq)
                .HasMaxLength(128)
                .HasColumnName("Name_SQ");
            entity.Property(e => e.NormalizedName).HasMaxLength(256);
        });

        modelBuilder.Entity<AspNetUserClaims>(entity =>
        {
            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserClaims).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserLogins>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserLogins).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUserTokens>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            entity.HasOne(d => d.User).WithMany(p => p.AspNetUserTokens).HasForeignKey(d => d.UserId);
        });

        modelBuilder.Entity<AspNetUsers>(entity =>
        {
            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                .IsUnique()
                .HasFilter("([NormalizedUserName] IS NOT NULL)");

            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.Email).HasMaxLength(256);
            entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            entity.Property(e => e.NormalizedEmail).HasMaxLength(256);
            entity.Property(e => e.NormalizedUserName).HasMaxLength(256);
            entity.Property(e => e.SettlementId).HasColumnName("SettlementID");
            entity.Property(e => e.UserName).HasMaxLength(256);

            entity.HasOne(d => d.Language).WithMany(p => p.AspNetUsers)
                .HasForeignKey(d => d.LanguageId)
                .HasConstraintName("FK_AspNetUsers_Language");

            entity.HasMany(d => d.Role).WithMany(p => p.User)
                .UsingEntity<Dictionary<string, object>>(
                    "AspNetUserRoles",
                    r => r.HasOne<AspNetRoles>().WithMany().HasForeignKey("RoleId"),
                    l => l.HasOne<AspNetUsers>().WithMany().HasForeignKey("UserId"),
                    j =>
                    {
                        j.HasKey("UserId", "RoleId");
                        j.HasIndex(new[] { "RoleId" }, "IX_AspNetUserRoles_RoleId");
                    });
        });

        modelBuilder.Entity<Classes>(entity =>
        {
            entity.HasKey(e => e.ClassId).HasName("PK__Classes__CB1927A02A82E515");

            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.ClassName).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ScheduleDay).HasMaxLength(10);
        });

        modelBuilder.Entity<Exercises>(entity =>
        {
            entity.HasKey(e => e.ExerciseId).HasName("PK__Exercise__A074AD0F48FEF79A");

            entity.Property(e => e.ExerciseId).HasColumnName("ExerciseID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.DifficultyLevel).HasMaxLength(20);
            entity.Property(e => e.ExerciseName).HasMaxLength(100);
            entity.Property(e => e.MuscleGroup).HasMaxLength(50);
        });

        modelBuilder.Entity<Language>(entity =>
        {
            entity.HasKey(e => e.LanguageId).HasName("PK__Languge__B938558BB0A1FDCF");

            entity.Property(e => e.LanguageId).HasColumnName("LanguageID");
            entity.Property(e => e.NameEn)
                .HasMaxLength(100)
                .HasColumnName("NameEN");
            entity.Property(e => e.NameSq)
                .HasMaxLength(50)
                .HasColumnName("NameSQ");
            entity.Property(e => e.Notes).HasMaxLength(255);
        });

        modelBuilder.Entity<Logs>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Logs__3214EC07EB0E13EF");

            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.Controller).HasMaxLength(100);
            entity.Property(e => e.HttpMethod).HasMaxLength(10);
            entity.Property(e => e.InsertedDate).HasColumnType("datetime");
            entity.Property(e => e.Ip).HasMaxLength(50);
            entity.Property(e => e.Url).HasMaxLength(200);
            entity.Property(e => e.UserId).HasMaxLength(450);

            entity.HasOne(d => d.User).WithMany(p => p.Logs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Logs_UserId");
        });

        modelBuilder.Entity<MembershipTypes>(entity =>
        {
            entity.HasKey(e => e.MembershipTypeId).HasName("PK__Membersh__F35A3E5965F0D49D");

            entity.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Memberships>(entity =>
        {
            entity.HasKey(e => e.MembershipId).HasName("PK__Membersh__92A78599B1A25C56");

            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MembershipTypeId).HasColumnName("MembershipTypeID");
            entity.Property(e => e.StartDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.MembershipType).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.MembershipTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__Membe__73BA3083");

            entity.HasOne(d => d.User).WithMany(p => p.Memberships)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Membershi__UserI__72C60C4A");
        });

        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Menu__3214EC076025AA71");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(450);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.NameSq).HasMaxLength(100);
            entity.Property(e => e.NameSr).HasMaxLength(100);
            entity.Property(e => e.Path).HasMaxLength(200);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.MenuCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Menu_CreatedBy");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.MenuModifiedByNavigation)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_Menu_ModifiedBy");

            entity.HasOne(d => d.Parent).WithMany(p => p.InverseParent)
                .HasForeignKey(d => d.ParentId)
                .HasConstraintName("FK_Menu_ParentId");
        });

        modelBuilder.Entity<Payments>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payments__9B556A589CE31E65");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.MembershipId).HasColumnName("MembershipID");
            entity.Property(e => e.Notes).HasMaxLength(255);
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.TransactionId)
                .HasMaxLength(100)
                .HasColumnName("TransactionID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Membership).WithMany(p => p.Payments)
                .HasForeignKey(d => d.MembershipId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Member__778AC167");

            entity.HasOne(d => d.User).WithMany(p => p.Payments)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__UserID__76969D2E");
        });

        modelBuilder.Entity<RoleMenus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RoleMenu__3214EC072959D2D2");

            entity.Property(e => e.Created).HasColumnType("datetime");
            entity.Property(e => e.CreatedBy).HasMaxLength(450);
            entity.Property(e => e.Modified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(450);
            entity.Property(e => e.RoleId).HasMaxLength(450);

            entity.HasOne(d => d.CreatedByNavigation).WithMany(p => p.RoleMenusCreatedByNavigation)
                .HasForeignKey(d => d.CreatedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleMenus_CreatedBy");

            entity.HasOne(d => d.Menu).WithMany(p => p.RoleMenus)
                .HasForeignKey(d => d.MenuId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleMenus_MenuId");

            entity.HasOne(d => d.ModifiedByNavigation).WithMany(p => p.RoleMenusModifiedByNavigation)
                .HasForeignKey(d => d.ModifiedBy)
                .HasConstraintName("FK_RoleMenus_ModifiedBy");

            entity.HasOne(d => d.Role).WithMany(p => p.RoleMenus)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RoleMenus_RoleId");
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(e => e.ScheduleId).HasName("PK__Schedule__9C8A5B69AC6F8FB5");

            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.DayOfWeek).HasMaxLength(10);
            entity.Property(e => e.EndTime).HasColumnType("datetime");
            entity.Property(e => e.StartTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<StatusType>(entity =>
        {
            entity.HasKey(e => e.StatusTypeId).HasName("PK__StatusTy__A84F3C734D3330DE");

            entity.Property(e => e.InsertedDate).HasColumnType("datetime");
            entity.Property(e => e.InsertedFrom).HasMaxLength(450);
            entity.Property(e => e.NameEn).HasMaxLength(100);
            entity.Property(e => e.NameSq).HasMaxLength(100);
            entity.Property(e => e.NameSr).HasMaxLength(100);
            entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            entity.Property(e => e.UpdatedFrom).HasMaxLength(450);

            entity.HasOne(d => d.InsertedFromNavigation).WithMany(p => p.StatusTypeInsertedFromNavigation)
                .HasForeignKey(d => d.InsertedFrom)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StatusType_InsertedFrom");

            entity.HasOne(d => d.UpdatedFromNavigation).WithMany(p => p.StatusTypeUpdatedFromNavigation)
                .HasForeignKey(d => d.UpdatedFrom)
                .HasConstraintName("FK_StatusType_UpdatedFrom");
        });

        modelBuilder.Entity<Test>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__Test__8CC33100F58A8C40");

            entity.Property(e => e.TestId).HasColumnName("TestID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserClasses>(entity =>
        {
            entity.HasKey(e => e.UserClassId).HasName("PK__UserClas__151870CF1013FE44");

            entity.Property(e => e.UserClassId).HasColumnName("UserClassID");
            entity.Property(e => e.ClassId).HasColumnName("ClassID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Class).WithMany(p => p.UserClasses)
                .HasForeignKey(d => d.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserClass__Class__7D439ABD");

            entity.HasOne(d => d.User).WithMany(p => p.UserClasses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserClass__UserI__7C4F7684");
        });

        modelBuilder.Entity<UsersSchedule>(entity =>
        {
            entity.HasKey(e => e.UserScheduleId).HasName("PK__UsersSch__9659B0319F77889C");

            entity.Property(e => e.UserScheduleId).HasColumnName("UserScheduleID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.UserId)
                .HasMaxLength(450)
                .HasColumnName("UserID");

            entity.HasOne(d => d.Schedule).WithMany(p => p.UsersSchedule)
                .HasForeignKey(d => d.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersSche__Sched__31B762FC");

            entity.HasOne(d => d.User).WithMany(p => p.UsersSchedule)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UsersSche__UserI__30C33EC3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
