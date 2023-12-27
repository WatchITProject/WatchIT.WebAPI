using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchIT.WebAPI.Services.Accounts.Configurations
{
    public interface IJWTConfiguration
    {
        int AccessTokenLifetime { get; }
        string Issuer { get; }
        byte[] Key { get; }
        int RefreshTokenExtendedLifetime { get; }
        int RefreshTokenNormalLifetime { get; }
    }

    public class JWTConfiguration : IJWTConfiguration
    {
        #region PROPERTIES

        public byte[] Key { get; protected set; }
        public string Issuer { get; protected set; }
        public int AccessTokenLifetime { get; protected set; }
        public int RefreshTokenNormalLifetime { get; protected set; }
        public int RefreshTokenExtendedLifetime { get; protected set; }

        #endregion



        #region CONSTRUCTORS

        public JWTConfiguration(IConfiguration configuration)
        {
            Key = Encoding.UTF8.GetBytes(configuration.GetRequiredSection("JWT:Key").Value!);
            Issuer = configuration.GetRequiredSection("JWT:Issuer").Value!;
            AccessTokenLifetime = int.Parse(configuration.GetRequiredSection("JWT:AccessTokenLifetime").Value!);
            RefreshTokenNormalLifetime = int.Parse(configuration.GetRequiredSection("JWT:RefreshTokenNormalLifetime").Value!);
            RefreshTokenExtendedLifetime = int.Parse(configuration.GetRequiredSection("JWT:RefreshTokenExtendedLifetime").Value!);
        }

        #endregion
    }
}
