using AutoMapper;
using Illyrian.Domain.Entities;
using Illyrian.RestApi.Utils.General;

namespace Illyrian.RestApi.AutoMapper;

public class OutputMappings : Profile
{
    public OutputMappings()
    {
        CreateMap<User, Illyrian.Persistence.Administration.User.UserDto>()
            .ForMember(d => d.FullName, opt => opt.MapFrom(s => $"{s.Firstname ?? ""} {s.Lastname ?? ""}".Trim()))
            .ForMember(d => d.FormattedBirthdate, opt => opt.MapFrom(s => s.Birthdate.HasValue ? FormattingHelpers.FormatDate(s.Birthdate.Value) : null))
            .ForMember(d => d.FormattedInsertedDate, opt => opt.MapFrom(s => FormattingHelpers.FormatDate(s.InsertedDate)))
            .ForMember(d => d.Roles, opt => opt.MapFrom(s => s.Roles != null ? s.Roles.Select(r => r.Name ?? "").ToList() : new List<string>()));

        CreateMap<Role, Illyrian.Persistence.Administration.User.RoleDto>();

        CreateMap<Membership, Illyrian.Persistence.Membership.MembershipDto>()
            .ForMember(d => d.MembershipTypeName, opt => opt.MapFrom(s => s.MembershipType != null ? s.MembershipType.Name : null))
            .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive ?? false))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.MembershipType != null ? s.MembershipType.Price : 0))
            .ForMember(d => d.DurationInDays, opt => opt.MapFrom(s => s.MembershipType != null ? s.MembershipType.DurationInDays : 0))
            .ForMember(d => d.FormattedPrice, opt => opt.MapFrom(s => FormattingHelpers.FormatPrice(s.MembershipType != null ? s.MembershipType.Price : 0)))
            .ForMember(d => d.FormattedStartDate, opt => opt.MapFrom(s => FormattingHelpers.FormatDate(s.StartDate)))
            .ForMember(d => d.FormattedEndDate, opt => opt.MapFrom(s => FormattingHelpers.FormatDate(s.EndDate)))
            .ForMember(d => d.FormattedDuration, opt => opt.MapFrom(s => FormattingHelpers.FormatDuration(s.MembershipType != null ? s.MembershipType.DurationInDays : 0)));

        CreateMap<MembershipType, Illyrian.Persistence.MembershipType.MembershipTypeDto>()
            .ForMember(d => d.MembershipTypeID, opt => opt.MapFrom(s => s.MembershipTypeId))
            .ForMember(d => d.FormattedDuration, opt => opt.MapFrom(s => FormattingHelpers.FormatDuration(s.DurationInDays)))
            .ForMember(d => d.FormattedPrice, opt => opt.MapFrom(s => FormattingHelpers.FormatPrice(s.Price)));

        CreateMap<Payment, Illyrian.Persistence.Payment.PaymentDto>()
            .ForMember(d => d.MembershipTypeName, opt => opt.MapFrom(s => s.Membership != null && s.Membership.MembershipType != null ? s.Membership.MembershipType.Name : null))
            .ForMember(d => d.FormattedAmount, opt => opt.MapFrom(s => FormattingHelpers.FormatPrice(s.Amount)))
            .ForMember(d => d.FormattedPaymentDate, opt => opt.MapFrom(s => FormattingHelpers.FormatDate(s.PaymentDate)));

        CreateMap<Schedule, Illyrian.Persistence.Schedule.ScheduleDto>()
            .ForMember(d => d.FormattedStartTime, opt => opt.MapFrom(s => FormattingHelpers.FormatTime(s.StartTime)))
            .ForMember(d => d.FormattedEndTime, opt => opt.MapFrom(s => FormattingHelpers.FormatTime(s.EndTime)))
            .ForMember(d => d.FormattedDayOfWeek, opt => opt.MapFrom(s => s.DayOfWeek));

        CreateMap<Exercise, Illyrian.Persistence.Exercise.ExerciseDto>();
    }
}
