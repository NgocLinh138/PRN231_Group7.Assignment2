using PRN231_Group7.Assignment2.API.Mapper;
using PRN231_Group7.Assignment2.Repo.Repository;
using PRN231_Group7.Assignment2.Repo.Repository.Interface;
using PRN231_Group7.Assignment2.Repo.Services;
using PRN231_Group7.Assignment2.Repo.Services.Interface;

namespace PRN231_Group7.Assignment2.API.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigurationRepo(this IServiceCollection services)
            => services.AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>))
                .AddTransient(typeof(BookRepository))
                .AddTransient<IJWTTokenService, JWTTokenService>();

        public static IServiceCollection AddConfigurationMapper(this IServiceCollection services)
            => services.AddAutoMapper(typeof(ServiceProfile));

    }
}
