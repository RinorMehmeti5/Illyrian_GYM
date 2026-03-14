using Illyrian.Domain.Entities;

namespace Illyrian.Domain.Repositories;

public interface IApplicationDbContext
{
    IQueryable<Membership> Memberships { get; }
    IQueryable<MembershipType> MembershipTypes { get; }
    IQueryable<Payment> Payments { get; }
    IQueryable<GymClass> GymClasses { get; }
    IQueryable<Exercise> Exercises { get; }
    IQueryable<Schedule> Schedules { get; }
    IQueryable<UserClass> UserClasses { get; }
    IQueryable<UserSchedule> UserSchedules { get; }
    IQueryable<Language> Languages { get; }
    IQueryable<Menu> Menus { get; }
    IQueryable<RoleMenu> RoleMenus { get; }
    IQueryable<StatusType> StatusTypes { get; }
    IQueryable<LogEntry> Logs { get; }
    IQueryable<TestItem> TestItems { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
