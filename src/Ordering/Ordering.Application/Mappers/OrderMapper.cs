using AutoMapper;
using System;

namespace Ordering.Application.Mappers
{
    public class OrderMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<OrderMappingProfile>();
            });

            return config.CreateMapper();
        });

        public static IMapper Mapper => Lazy.Value;
    }
}
