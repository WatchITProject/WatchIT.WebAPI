using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WatchIT.WebAPI.Helpers;
using WatchIT.WebAPI.Model.Models;
using WatchIT.WebAPI.Services.AccountsService.Request;
using WatchIT.WebAPI.Services.AccountsService.Response;

namespace WatchIT.WebAPI.Services.AccountsService
{
    public class AccountsService : IAccountsService
    {
        #region FIELDS

        private DatabaseContext _database;
        private IConfiguration _configuration;

        #endregion



        #region CONSTRUCTORS

        public AccountsService(IDbContextFactory<DatabaseContext> database, IConfiguration configuration) 
        {
            _database = database.CreateDbContext();
            _configuration = configuration;
        }

        #endregion



        #region PUBLIC METHODS

        public void Register(RegisterRequest request)
        {
            if (request is null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (string.IsNullOrWhiteSpace(request.Email))
            {
                throw new Exception("Email cannot be empty");
            }

            if (_database.Accounts.Any(x => string.Equals(x.AccEmail, request.Email)))
            {
                throw new Exception("Provided email is used");
            }

            try
            {
                MailAddress m = new MailAddress(request.Email);
            }
            catch (FormatException ex)
            {
                throw new Exception("Invalid email");
            }

            if (string.IsNullOrWhiteSpace(request.Username))
            {
                throw new Exception("Username cannot be empty");
            }

            if (_database.Accounts.Any(x => string.Equals(x.AccName, request.Username)))
            {
                throw new Exception("Provided username is used");
            }

            IEnumerable<string> usernameChecks = CheckUsername(request.Password);
            if (usernameChecks.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Provided username does not meet the requirements:");
                foreach (string check in usernameChecks)
                {
                    sb.AppendLine(check);
                }
                throw new Exception(sb.ToString());
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new Exception("Password cannot be empty");
            }

            IEnumerable<string> passwordChecks = CheckPassword(request.Password);
            if (passwordChecks.Any())
            { 
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Provided password does not meet the security requirements:");
                foreach (string check in passwordChecks)
                {
                    sb.AppendLine(check);
                }
                throw new Exception(sb.ToString());
            }

            string salt = StringHelpers.CreateRandom(20);
            byte[] hash = HashPassword(request.Password, salt);

            Account account = new Account()
            {
                AccName = request.Username,
                AccEmail = request.Email,
                AccPassword = hash,
                AccSalt = salt
            };
            _database.Accounts.Add(account);
            _database.SaveChanges();
        }

        public AuthenticateResponse Authenticate(string emailOrUsername, string password)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.UTF8.GetBytes(_configuration.GetRequiredSection("JwtSettings:Key").Value!);

            Account? account = _database.Accounts.Where(x => x.AccEmail.Equals(emailOrUsername) || x.AccName.Equals(emailOrUsername)).FirstOrDefault();
            if (account is null)
            {
                throw new Exception("User with provided email or username not exists");
            }

            byte[] hashToCheck = HashPassword(password, account.AccSalt);
            if (!Enumerable.SequenceEqual(hashToCheck, account.AccPassword))
            {
                throw new Exception("Wrong password");
            }


            List<Claim> claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.AccId.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.AccEmail),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.AccName),
                new Claim("admin", account.AccAdmin.ToString().ToLower()),
            };

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(int.Parse(_configuration.GetRequiredSection("JwtSettings:Lifetime").Value!)),
                Issuer = _configuration.GetRequiredSection("JwtSettings:Issuer").Value!,
                Audience = _configuration.GetRequiredSection("JwtSettings:Audience").Value!,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512)
            };

            SecurityToken token = handler.CreateToken(tokenDescriptor);

            string jwt = handler.WriteToken(token);

            return new AuthenticateResponse() { Token = jwt };
        }

        #endregion



        #region PRIVATE METHODS

        protected byte[] HashPassword(string password, string salt) => SHA512.Create().ComputeHash(Encoding.UTF8.GetBytes($"{salt}{password}"));

        protected IEnumerable<string> CheckUsername(string username)
        {
            int minLength = 3;

            if (username.Length < minLength)
            {
                yield return $"Username must be at least {minLength} characters long";
            }
            if (!username.All(x => Char.IsLetterOrDigit(x)))
            {
                yield return $"Username cannot contains special characters and whitespaces";
            }
        }

        protected IEnumerable<string> CheckPassword(string password)
        {
            int minLength = 8;

            if (password.Length < minLength)
            {
                yield return $"Password must be at least {minLength} characters long";
            }
            if (!password.Any(x => Char.IsUpper(x)))
            {
                yield return $"Password must contain at least one uppercase character";
            }
            if (!password.Any(x => Char.IsLower(x)))
            {
                yield return $"Password must contain at least one lowercase character";
            }
            if (!password.Any(x => Char.IsDigit(x)))
            {
                yield return $"Password must contain at least one digit";
            }
        }

        #endregion
    }
}
