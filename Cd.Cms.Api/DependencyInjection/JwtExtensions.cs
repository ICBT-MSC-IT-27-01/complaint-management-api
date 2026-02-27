using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cd.Cms.Api.DependencyInjection
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration config)
        {
            var jwt = config.GetSection("Jwt");
            var key = jwt["Key"] ?? throw new InvalidOperationException("Jwt:Key missing in appsettings.json");

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer           = true,
                        ValidateAudience         = true,
                        ValidateIssuerSigningKey  = true,
                        ValidateLifetime         = true,
                        ValidIssuer              = jwt["Issuer"],
                        ValidAudience            = jwt["Audience"],
                        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                        ClockSkew                = TimeSpan.FromSeconds(30)
                    };
                });

            services.AddAuthorization();
            return services;
        }
    }
}
