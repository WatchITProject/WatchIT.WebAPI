using Azure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WatchIT.Common;
using WatchIT.Common.Accounts.Request;
using WatchIT.Common.Accounts.Response;
using WatchIT.WebAPI.Database;
using WatchIT.WebAPI.Helpers;
using WatchIT.WebAPI.Services.Accounts.Configurations;
using WatchIT.WebAPI.Services.Accounts.Helpers;
using WatchIT.WebAPI.Services;

namespace WatchIT.WebAPI.Services.Accounts
{
    public interface IAccountsService
    {
        Task<ApiResponse> Register(RegisterRequest request);
        Task<ApiResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest request);
        Task<ApiResponse<AuthenticateResponse>> AuthenticateRefresh(IEnumerable<Claim> claims);
        Task<ApiResponse> Logout(IEnumerable<Claim> claims);
        Task<ApiResponse> LogoutFromAllDevices(IEnumerable<Claim> claims);
    }

    public class AccountsService : IAccountsService
    {
        #region FIELDS

        private DatabaseContext _database;
        private IJWTHelper _jwtHelper;

        #endregion



        #region CONSTRUCTORS

        public AccountsService(IDbContextFactory<DatabaseContext> database, IJWTHelper jwtHelper) 
        {
            _database = database.CreateDbContext();
            _jwtHelper = jwtHelper;
        }

        #endregion



        #region PUBLIC METHODS

        public async Task<ApiResponse> Register(RegisterRequest request)
        {
            Check<RegisterRequest>[] checks = new Check<RegisterRequest>[]
            {
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) => req is null),
                    Message = "Body cannot be empty"
                },
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) => string.IsNullOrWhiteSpace(req.Email)),
                    Message = "Email cannot be empty"
                },
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) => _database.Account.Any(x => string.Equals(x.Email, req.Email))),
                    Message = "Provided email is used"
                },
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) =>
                    {
                        try
                        {
                            MailAddress m = new MailAddress(request.Email);
                        }
                        catch (FormatException ex)
                        {
                            return true;
                        }
                        return false;
                    }),
                    Message = "Invalid email"
                },
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) => string.IsNullOrWhiteSpace(req.Username)),
                    Message = "Username cannot be empty"
                },
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) => _database.Account.Any(x => string.Equals(x.Username, req.Username))),
                    Message = "Username is used"
                },
                new Check<RegisterRequest>
                {
                    CheckAction = new Predicate<RegisterRequest>((req) => string.IsNullOrWhiteSpace(req.Password)),
                    Message = "Password cannot be empty"
                },
            };

            foreach (Check<RegisterRequest> check in checks)
            {
                if (check.CheckAction.Invoke(request))
                {
                    return new ApiResponse<AuthenticateResponse>
                    {
                        Message = check.Message,
                        Success = false
                    };
                }
            }

            IEnumerable<string> usernameChecks = CheckUsername(request.Username);
            if (usernameChecks.Any())
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Provided username does not meet the requirements:");
                foreach (string check in usernameChecks)
                {
                    sb.AppendLine(check);
                }
                return new ApiResponse
                {
                    Success = false,
                    Message = sb.ToString()
                };
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
                return new ApiResponse
                {
                    Success = false,
                    Message = sb.ToString()
                };
            }

            string salt = StringHelpers.CreateRandom(20);
            byte[] hash = HashPassword(request.Password, salt);

            Account account = new Account()
            {
                Username = request.Username,
                Email = request.Email,
                Password = hash,
                Salt = salt
            };
            _database.Account.Add(account);
            await _database.SaveChangesAsync();

            return new ApiResponse
            {
                Success = true
            };
        }

        public async Task<ApiResponse<AuthenticateResponse>> Authenticate(AuthenticateRequest request)
        {
            Check<AuthenticateRequest>[] checks =
            [
                new Check<AuthenticateRequest>
                {
                    CheckAction = new Predicate<AuthenticateRequest>((req) => string.IsNullOrWhiteSpace(req.UsernameOrEmail)),
                    Message = "Username or email must be provided"
                },
                new Check<AuthenticateRequest>
                {
                    CheckAction = new Predicate<AuthenticateRequest>((req) => string.IsNullOrWhiteSpace(req.Password)),
                    Message = "Password must be provided"
                },
                new Check<AuthenticateRequest>
                {
                    CheckAction = new Predicate<AuthenticateRequest>((req) => !_database.Account.Any(x => x.Email.Equals(req.UsernameOrEmail) || x.Username.Equals(req.UsernameOrEmail))),
                    Message = "User with provided email or username not exists"
                },
                new Check<AuthenticateRequest>
                {
                    CheckAction = new Predicate<AuthenticateRequest>((req) =>
                    {
                        Account account = _database.Account.FirstOrDefault(x => x.Email.Equals(req.UsernameOrEmail) || x.Username.Equals(req.UsernameOrEmail))!;
                        byte[] hashToCheck = HashPassword(req.Password, account.Salt);
                        return !Enumerable.SequenceEqual(hashToCheck, account.Password);
                    }),
                    Message = "Incorrect password"
                },
            ];

            foreach (Check<AuthenticateRequest> check in checks)
            {
                if (check.CheckAction.Invoke(request))
                {
                    return new ApiResponse<AuthenticateResponse>
                    {
                        Message = check.Message,
                        Success = false
                    };
                }
            }

            Account account = _database.Account.FirstOrDefault(x => x.Email.Equals(request.UsernameOrEmail) || x.Username.Equals(request.UsernameOrEmail))!;

            Task<string> refreshTokenTask = _jwtHelper.GenerateRefreshToken(account, request.RememberMe);
            Task<string> accessTokenTask = _jwtHelper.GenerateAccessToken(account);
            await Task.WhenAll(refreshTokenTask, accessTokenTask);

            return new ApiResponse<AuthenticateResponse>
            {
                Success = true,
                Data = new AuthenticateResponse
                {
                    RefreshToken = refreshTokenTask.Result,
                    AccessToken = accessTokenTask.Result,
                }
            };
        }

        public async Task<ApiResponse<AuthenticateResponse>> AuthenticateRefresh(IEnumerable<Claim> claims)
        {
            foreach (Claim claim in claims)
            {
                Debug.WriteLine($"{claim.Type}, {claim.Value}");
            }
            Check<IEnumerable<Claim>>[] checks = new Check<IEnumerable<Claim>>[]
            {
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => arg is null),
                    Message = "Token is invalid: claims collection is null"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Any()),
                    Message = "Token is invalid: claims collection is empty"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Select(x => x.Type).Contains(JwtRegisteredClaimNames.Jti)),
                    Message = "Token is invalid: claims collection does not contain \"jti\" claim"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Select(x => x.Type).Contains(JwtRegisteredClaimNames.Sub)),
                    Message = "Token is invalid: claims collection does not contain \"sub\" claim"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Select(x => x.Type).Contains(JwtRegisteredClaimNames.Email)),
                    Message = "Token is invalid: claims collection does not contain \"email\" claim"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Select(x => x.Type).Contains(JwtRegisteredClaimNames.UniqueName)),
                    Message = "Token is invalid: claims collection does not contain \"unique_name\" claim"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Select(x => x.Type).Contains(JwtRegisteredClaimNames.Exp)),
                    Message = "Token is invalid: claims collection does not contain \"exp\" claim"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !arg.Select(x => x.Type).Contains("extend")),
                    Message = "Token is invalid: claims collection does not contain \"extend\" claim"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) => !Convert.ToBoolean(arg.FirstOrDefault(x => x.Type == "extend").Value)),
                    Message = "Token is not extendable"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) =>
                    {
                        int userId = int.Parse(arg.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!.Value);
                        Account? account = _database.Account.FirstOrDefault(x => x.Id == userId);
                        return account is null;
                    }),
                    Message = "User assigned to this token does not exist"
                },
                new Check<IEnumerable<Claim>>
                {
                    CheckAction = new Predicate<IEnumerable<Claim>>((arg) =>
                    {
                        Guid tokenId = Guid.Parse(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value);
                        int userId = int.Parse(arg.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!.Value);
                        Account? account = _database.Account.FirstOrDefault(x => x.Id == userId);
                        return !_database.AccountRefreshToken.Where(x =>x.AccountId == userId).Any(x => x.Id.CompareTo(tokenId) == 0);
                    }),
                    Message = "Refresh token is invalid for this user"
                },
            };

            foreach (Check<IEnumerable<Claim>> check in checks)
            {
                if (check.CheckAction.Invoke(claims))
                {
                    return new ApiResponse<AuthenticateResponse>
                    {
                        Message = check.Message,
                        Success = false
                    };
                }
            }

            Guid tokenId = Guid.Parse(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)!.Value);
            int userId = int.Parse(claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Sub)!.Value);

            Account account = _database.Account.FirstOrDefault(x => x.Id == userId);
            AccountRefreshToken token = _database.AccountRefreshToken.FirstOrDefault(x => x.Id == tokenId);
            _database.AccountRefreshToken.Attach(token);
            _database.AccountRefreshToken.Remove(token);
            await _database.SaveChangesAsync();

            Task<string> refreshTokenTask = _jwtHelper.GenerateRefreshToken(account, true);
            Task<string> accessTokenTask = _jwtHelper.GenerateAccessToken(account);
            await Task.WhenAll(refreshTokenTask, accessTokenTask);

            return new ApiResponse<AuthenticateResponse>
            {
                Success = true,
                Data = new AuthenticateResponse
                {
                    RefreshToken = refreshTokenTask.Result,
                    AccessToken = accessTokenTask.Result,
                }
            };
        }

        public async Task<ApiResponse> Logout(IEnumerable<Claim> claims)
        {
            return new ApiResponse();
        }

        public async Task<ApiResponse> LogoutFromAllDevices(IEnumerable<Claim> claims)
        {
            return new ApiResponse();
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
