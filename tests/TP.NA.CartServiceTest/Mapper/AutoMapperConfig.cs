using AutoMapper;
using TP.NA.CartService.Application.Commands.Cart;
using TP.NA.CartService.Application.Config;

namespace TP.NA.CartServiceTest.MapperConfig
{
    public class AutoMapperConf
    {
        public static MapperConfiguration Configuration()
        {
            List<Profile> profileList = new List<Profile>
            {
                new AutoMapperConfig(),
                new CreateCartCommand.Mapper()
            };

            var config = new MapperConfiguration(cfg => cfg.AddProfiles(profileList));

            return config;
        }
    }
}