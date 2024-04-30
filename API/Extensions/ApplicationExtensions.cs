﻿using API.Data;
using API.Helpers;
using API.Interfaces;
using API.Services;
using API.Utility;
using AutoMapper;
using FluentValidation;
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

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
            builder.Services.AddScoped<IEmailSender, EmailSender>();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddValidatorsFromAssemblyContaining<IAssemblyMarker>();
            builder.Services.Configure<JwtOptions>(configuration.GetSection("JwtOptions"));

            return builder;
        }
    }
}
