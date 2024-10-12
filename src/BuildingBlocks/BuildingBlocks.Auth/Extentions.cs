using System.Text;
using BuildingBlocks.Auth.Attributes;
using BuildingBlocks.Auth.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace BuildingBlocks.Auth;

public record TokenConfig(string SigningKey,
    double AccessTokenExp);


public static class Extentions
{
    public static IServiceCollection AddAuthentication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var tokenConfig = configuration.GetSection(nameof(TokenConfig))
            .Get<TokenConfig>() ?? throw new Exception($"{nameof(TokenConfig)} isn't found.");

        if (!services.Any(d => d.ServiceType == typeof(IDistributedCache)))
            services.AddStackExchangeRedisCache(options =>
                options.Configuration = configuration.GetConnectionString("Redis"));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Events = new()
                {
                    OnTokenValidated = async context =>
                    {
                        var cache = context.HttpContext.RequestServices
                            .GetRequiredService<IDistributedCache>();
                        var tokenId = context.SecurityToken.Id;
                        var isTokenBlacklisted = await cache.GetStringAsync($"blacklist:{tokenId}") != null;
                        if (isTokenBlacklisted)
                            context.Fail("Token is blacklisted");
                    }
                };

                options.TokenValidationParameters = new()
                {
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(tokenConfig.SigningKey)
                    )
                };
            });
        return services;
    }

    public static TBuilder RequirePermission<TBuilder>(this TBuilder builder)
        where TBuilder : IEndpointConventionBuilder
    {
        builder.Add(endpointBuilder =>
            endpointBuilder.Metadata.Add(new RequirePermissionAttribute())
        );

        return builder;
    }

    public static IApplicationBuilder UseCustomAuthorization(this IApplicationBuilder builder)
    {
        builder.UseMiddleware<AuthorizationMiddleware>();
        return builder;
    }
}