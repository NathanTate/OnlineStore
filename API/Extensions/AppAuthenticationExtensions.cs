using API.Data;
using API.Models;
using API.Utility;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API.Extensions
{
    public static class AppAuthenticationExtensions
    {
        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            var JwtOptions = new JwtOptions();
            configuration.Bind("JwtOptions", JwtOptions);
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireDigit = true;
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidAudience = JwtOptions.Audience,
                        ValidIssuer = JwtOptions.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("JwtOptions:SecretKey"))),
                        ClockSkew = TimeSpan.FromSeconds(0),
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            context.Request.Cookies.TryGetValue("accessToken", out string accessToken);
                            if (!string.IsNullOrWhiteSpace(accessToken))
                                context.Token = accessToken;

                            return Task.CompletedTask;
                        }
                    };
                });
            //.AddCookie();
            builder.Services.AddAuthorization();
            return builder;
        }
    }
}
