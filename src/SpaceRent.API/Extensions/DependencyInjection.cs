using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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

        // Identity
        services.AddIdentity<SpaceRent.Domain.Entities.User, SpaceRent.Domain.Entities.Role>()
            .AddEntityFrameworkStores<SpaceRentDbContext>()
            .AddDefaultTokenProviders();

        // Auth Settings
        services.Configure<SpaceRent.Infrastructure.Authentication.JwtSettings>(configuration.GetSection(SpaceRent.Infrastructure.Authentication.JwtSettings.SectionName));
        services.Configure<SpaceRent.Infrastructure.Email.ResendSettings>(configuration.GetSection(SpaceRent.Infrastructure.Email.ResendSettings.SectionName));

        // Authentication & JWT
        var jwtSettings = configuration.GetSection(SpaceRent.Infrastructure.Authentication.JwtSettings.SectionName).Get<SpaceRent.Infrastructure.Authentication.JwtSettings>();
        if (jwtSettings != null)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                        System.Text.Encoding.UTF8.GetBytes(jwtSettings.Secret))
                };
            });
        }

        // Services
        services.AddScoped<IJwtTokenGenerator, SpaceRent.Infrastructure.Authentication.JwtTokenGenerator>();
        services.AddScoped<IGoogleAuthService, SpaceRent.Infrastructure.Authentication.GoogleAuthService>();
        services.AddScoped<IEmailService, SpaceRent.Infrastructure.Email.ResendEmailService>();
        
        // Resend
        services.AddHttpClient<Resend.IResend, Resend.ResendClient>();
        services.Configure<Resend.ResendClientOptions>(options =>
        {
            options.ApiToken = configuration.GetSection(SpaceRent.Infrastructure.Email.ResendSettings.SectionName)["ApiKey"] ?? string.Empty;
        });

        // Application
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateSpaceCommand>());
        services.AddValidatorsFromAssemblyContaining<CreateSpaceCommandValidator>();

        return services;
    }
}
