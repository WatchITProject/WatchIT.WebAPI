using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using WatchIT.WebAPI.Database;
using WatchIT.WebAPI.Services.AccountsService;
using WatchIT.WebAPI.Services.AccountsService.Configurations;
using WatchIT.WebAPI.Services.AccountsService.Helpers;
using WatchIT.WebAPI.Services.MoviesService;

namespace WatchIT.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration files
            builder.Configuration.AddJsonFile("Config/appsettings.json", false, false);
            builder.Configuration.AddJsonFile("Config/connectionStrings.json", false, false);
            builder.Configuration.AddJsonFile("Config/jwt.json", false, false);

            // Authentication
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            AuthenticationBuilder auth = builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            auth.AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
                    ValidateAudience = true,
                    ValidAudience = "access",
                    ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
                x.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            auth.AddJwtBearer("refresh", x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JWT:Key"))),
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration.GetValue<string>("JWT:Issuer"),
                    ValidAudience = "refresh",
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(1)
                };
            });
            builder.Services.AddAuthorization();

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Database contexts
            builder.Services.AddDbContextFactory<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("database"));
            });

            // Configurations
            builder.Services.AddSingleton<IJWTConfiguration, JWTConfiguration>();

            // Helpers
            builder.Services.AddSingleton<IJWTHelper, JWTHelper>();

            // Services
            builder.Services.AddSingleton<IAccountsService, AccountsService>();
            builder.Services.AddSingleton<IMoviesService, MoviesService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}