using Microsoft.EntityFrameworkCore;
using FluentValidation;
using SpaceRent.Application.Interfaces;
using SpaceRent.Application.Spaces.Commands;
using SpaceRent.Application.Spaces.Validators;
using SpaceRent.Infrastructure.Data;
using SpaceRent.Infrastructure.Repositories;
using SpaceRent.Infrastructure.Services;

namespace SpaceRent.API.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddSpaceRentServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Infrastructure
        services.AddDbContext<SpaceRentDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<ISpaceRepository, SpaceRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBookingRepository, BookingRepository>();
        services.AddScoped<IPaymentRepository, PaymentRepository>();
        services.AddScoped<IAmenityRepository, AmenityRepository>();
        services.AddScoped<ISpaceMediaRepository, SpaceMediaRepository>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        // Application
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateSpaceCommand>());
        services.AddValidatorsFromAssemblyContaining<CreateSpaceCommandValidator>();

        return services;
    }
}
