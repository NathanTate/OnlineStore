using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.Utility;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions
{
    public static class ApplicationExtensions
    {
        public static WebApplicationBuilder AddApplicationExtensions(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            IMapper mapper = AutoMapperConfig.RegisterMaps().CreateMapper();
            builder.Services.AddSingleton(mapper);
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.Configure<DataProtectionTokenProviderOptions>(opts => opts.TokenLifespan = TimeSpan.FromHours(1));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IPhotoService, PhotoService>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            builder.Services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));
            builder.Services.Configure<CloudinaryOptions>(configuration.GetSection("CloudinarySettings"));

            return builder;
        }
    }
}
