using System.Runtime.CompilerServices;
using Cards.Application.Common.Services;
using Cards.Application.Users.Services;
using Cards.Infrastructure.Options;
using Cards.Infrastructure.Persistence;
using Cards.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("CardsTestE2E")]
namespace Cards.Infrastructure;

public static class ServiceRegistration
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), o => o.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
        //
        // services
        //     .AddDbContext<IApplicationDbContext, ApplicationDbContext>(opt => opt.UseInMemoryDatabase("CardsDb"));
        
        services
            .AddScoped<IDateTimeService, UtcNowService>()
            .AddSingleton<ITokenService, TokenGenerationService>()
            .AddSingleton<IHashingService, HmacSha512HashingService>();

        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("JwtOptions"))
            .ValidateOnStart();

        return services;
    }
}