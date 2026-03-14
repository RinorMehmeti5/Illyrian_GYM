using AutoMapper;
using Illyrian.Domain.AutoMapper;
using Illyrian.RestApi.AutoMapper;

namespace Illyrian.RestApi.Tests.Configuration;

public static class AutoMapperFixture
{
    private static IMapper? _mapper;

    public static IMapper Mapper
    {
        get
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<DomainMappingConfiguration>();
                    cfg.AddProfile<InputMappings>();
                    cfg.AddProfile<OutputMappings>();
                });
                _mapper = config.CreateMapper();
            }
            return _mapper;
        }
    }
}
