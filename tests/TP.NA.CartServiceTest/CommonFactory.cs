using AutoMapper;
using FluentValidation;
using MediatR;
using Moq;
using TP.NA.CartService.Application.Abstractions.Repository;
using TP.NA.CartService.Application.Commons;

namespace TP.NA.CartServiceTest
{
    /// <summary>
    ///
    /// </summary>
    public class CommonFactory
    {
        public static Mock<IMediator> GetMediatorMock => new Mock<IMediator>();

        public static Mapper GetMapper(MapperConfiguration config) => new Mapper(config);

        public static Mock<IMapper> GetMapperMock() => new Mock<IMapper>();

        public static Mock<IValidator<T>> GetValidatorMock<T>(T command) where T : CommonCommand => new Mock<IValidator<T>>();

        #region repositories

        public static Mock<ICartRepository> CartRepository => new Mock<ICartRepository>();

        #endregion repositories
    }
}