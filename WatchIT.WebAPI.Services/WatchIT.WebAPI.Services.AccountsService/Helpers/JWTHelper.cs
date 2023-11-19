using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WatchIT.WebAPI.Database;
using WatchIT.WebAPI.Services.AccountsService.Configurations;

namespace WatchIT.WebAPI.Services.AccountsService.Helpers
{
    public interface IJWTHelper
    {
        Task<string> GenerateAccessToken(Account account);
        Task<string> GenerateRefreshToken(Account account, bool extendable);
    }

    public class JWTHelper : IJWTHelper
    {
        #region FIELDS

        private DatabaseContext _database;
        private IJWTConfiguration _jwtConfiguration;

        #endregion



        #region CONSTRUCTORS

        public JWTHelper(IDbContextFactory<DatabaseContext> database, IJWTConfiguration jwtConfiguration)
        {
            _database = database.CreateDbContext();
            _jwtConfiguration = jwtConfiguration;
        }

        #endregion



        #region PUBLIC METHODS

        public async Task<string> GenerateRefreshToken(Account account, bool extendable)
        {
            int expirationMinutes = extendable ? _jwtConfiguration.RefreshTokenExtendedLifetime : _jwtConfiguration.RefreshTokenNormalLifetime;
            DateTime expirationTime = DateTime.Now.AddMinutes(expirationMinutes);
            Guid id = Guid.NewGuid();

            AccountRefreshToken refreshToken = new AccountRefreshToken
            {
                Id = id,
                AccountId = account.Id,
                ExpirationDate = expirationTime,
            };
            _database.AccountRefreshTokens.Add(refreshToken);
            Task saveTask = _database.SaveChangesAsync();

            SecurityTokenDescriptor tokenDescriptor = CreateBaseSecurityTokenDescriptor(account, id, expirationTime);
            tokenDescriptor.Audience = "refresh";
            tokenDescriptor.Subject.AddClaim(new Claim("extend", extendable.ToString()));

            string tokenString = TokenToString(tokenDescriptor);

            await saveTask;

            return tokenString;
        }

        public async Task<string> GenerateAccessToken(Account account)
        {
            return await Task.Run(() =>
            {
                DateTime expirationTime = DateTime.Now.AddMinutes(_jwtConfiguration.AccessTokenLifetime);
                Guid id = Guid.NewGuid();

                SecurityTokenDescriptor tokenDescriptor = CreateBaseSecurityTokenDescriptor(account, id, expirationTime);
                tokenDescriptor.Audience = "access";
                tokenDescriptor.Subject.AddClaim(new Claim("admin", account.Admin.ToString()));

                return TokenToString(tokenDescriptor);
            });
        }

        #endregion



        #region PRIVATE METHODS

        protected SecurityTokenDescriptor CreateBaseSecurityTokenDescriptor(Account account, Guid id, DateTime expirationTime) => new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Username),
                new Claim(JwtRegisteredClaimNames.Exp, expirationTime.ToString()),
            }),
            Expires = expirationTime,
            Issuer = _jwtConfiguration.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_jwtConfiguration.Key), SecurityAlgorithms.HmacSha512)
        };

        protected string TokenToString(SecurityTokenDescriptor tokenDescriptor)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            handler.InboundClaimTypeMap.Clear();

            SecurityToken token = handler.CreateToken(tokenDescriptor);

            return handler.WriteToken(token);
        }

        #endregion 
    }
}
