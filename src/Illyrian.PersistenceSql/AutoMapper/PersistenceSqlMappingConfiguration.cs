using AutoMapper;
using Illyrian.Domain.Entities;

namespace Illyrian.PersistenceSql.AutoMapper;

public class PersistenceSqlMappingConfiguration : Profile
{
    public PersistenceSqlMappingConfiguration()
    {
        CreateMap<Membership, Illyrian.Persistence.Membership.MembershipDto>()
            .ForMember(d => d.MembershipTypeName, opt => opt.MapFrom(s => s.MembershipType != null ? s.MembershipType.Name : null))
            .ForMember(d => d.IsActive, opt => opt.MapFrom(s => s.IsActive ?? false))
            .ForMember(d => d.Price, opt => opt.MapFrom(s => s.MembershipType != null ? s.MembershipType.Price : 0))
            .ForMember(d => d.DurationInDays, opt => opt.MapFrom(s => s.MembershipType != null ? s.MembershipType.DurationInDays : 0));

        CreateMap<MembershipType, Illyrian.Persistence.MembershipType.MembershipTypeDto>()
            .ForMember(d => d.MembershipTypeID, opt => opt.MapFrom(s => s.MembershipTypeId));

        CreateMap<Payment, Illyrian.Persistence.Payment.PaymentDto>()
            .ForMember(d => d.MembershipTypeName, opt => opt.MapFrom(s => s.Membership != null && s.Membership.MembershipType != null ? s.Membership.MembershipType.Name : null));

        CreateMap<Schedule, Illyrian.Persistence.Schedule.ScheduleDto>();

        CreateMap<Exercise, Illyrian.Persistence.Exercise.ExerciseDto>();
    }
}
