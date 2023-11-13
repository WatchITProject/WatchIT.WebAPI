using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Text;
using WatchIT.WebAPI.Model.Models;
using WatchIT.WebAPI.Services.AccountsService;

namespace WatchIT.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configuration files
            builder.Configuration.AddJsonFile("Config/appsettings.json", false, false);
            builder.Configuration.AddJsonFile("Config/connectionStrings.json");
            builder.Configuration.AddJsonFile("Config/jwtSettings.json");

            // Authentication
            AuthenticationBuilder auth = builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme);
            auth.AddJwtBearer(x =>
            {
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = builder.Configuration.GetValue<string>("JwtSettings:Issuer"),
                    ValidAudience = builder.Configuration.GetValue<string>("JwtSettings:Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("JwtSettings:Key"))),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
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

            // Services
            builder.Services.AddSingleton<IAccountsService, AccountsService>();

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