using AutoMapper;
using Illyrian.Domain.Services.Auth;
using Illyrian.Persistence.Auth;
using Illyrian.Persistence.Administration.User;

namespace Illyrian.RestApi.AutoMapper;

public class InputMappings : Profile
{
    public InputMappings()
    {
        CreateMap<RegisterRequest, RegisterModel>();
        CreateMap<CreateUserRequest, CreateUserModel>();
        CreateMap<UpdateUserRequest, UpdateUserModel>();
    }
}
