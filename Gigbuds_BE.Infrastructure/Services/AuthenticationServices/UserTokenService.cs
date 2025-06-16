using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Gigbuds_BE.Application.Interfaces.Services.AuthenticationServices;
using Gigbuds_BE.Domain.Entities.Identity;
using Gigbuds_BE.Domain.Entities.Memberships;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Gigbuds_BE.Infrastructure.Services.AuthenticationServices;

public class UserTokenService(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager
    ) : IUserTokenService
    {
        public string CreateAccessToken(ApplicationUser user, List<string> roles)
        {
            var jwtSettings = configuration.GetSection("JwtAccessTokenSettings");
            string secretKey = jwtSettings["Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    [
                        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, string.Join(",", roles)),
                    ]),
                Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("JwtAccessTokenSettings:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Audience = jwtSettings["Audience"],
                Issuer = jwtSettings["Issuer"]!,
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptior);

            return token;
        }

        public string CreateIdToken(ApplicationUser user, List<string> roles)
        {
            var jwtSettings = configuration.GetSection("JwtIDTokenSettings");
            string secretKey = jwtSettings["Secret"]!;
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var audiences = jwtSettings.GetSection("Audience").Get<string[]>() ?? [];


            var claimsList = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Birthdate, user.Dob.ToString()!),
                new Claim(JwtRegisteredClaimNames.PhoneNumber, user.PhoneNumber!),
                new Claim(JwtRegisteredClaimNames.Name, user.FirstName!),
                new Claim(JwtRegisteredClaimNames.FamilyName, user.LastName!),
                new Claim(JwtRegisteredClaimNames.Gender, user.IsMale.ToString()!),
                new Claim("role", string.Join(",", roles)),
                new Claim("aud", audiences[0].ToString()),
                new Claim("aud", audiences[1].ToString()),
                new Claim("aud", audiences[2].ToString()),
            };

            var activeMembership = user.AccountMemberships?.Where(am => am.Status == AccountMembershipStatus.Active && user.Id == am.AccountId).Select(am => new{
                Id = am.Id,
                Title = am.Membership.Title,
                Type = am.Membership.MembershipType.ToString(),
                StartDate = am.StartDate,
                EndDate = am.EndDate,
                Status = am.Status.ToString(),
                MembershipId = am.MembershipId
            }).ToList();

            if(activeMembership?.Any() == true)
            {
                var membershipClaimsJson = JsonSerializer.Serialize(activeMembership);
                claimsList.Add(new Claim("memberships", membershipClaimsJson));
            }

            var tokenDescriptior = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claimsList),
                Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("JwtIDTokenSettings:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = jwtSettings["Issuer"]!,
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptior);

            return token;
        }

        public string CreateRefreshToken(ApplicationUser user)
        {
            var jwtSettings = configuration.GetSection("JwtRefreshTokenSettings");

            var number = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(number);
            var securityKey = new SymmetricSecurityKey(number);

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Claims = new Dictionary<string, object>
                    {
                        { JwtRegisteredClaimNames.Sub, user.Id.ToString() }
                    },
                Expires = DateTime.Now.AddMinutes(configuration.GetValue<int>("JwtRefreshTokenSettings:ExpirationInMinutes")),
                SigningCredentials = credentials,
                Issuer = jwtSettings["Issuer"]!,
            };

            var handler = new JsonWebTokenHandler();

            string token = handler.CreateToken(tokenDescriptor);

            return token;
        }

        public async Task RevokeUserToken(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {userId} not found");
            }

            user.RefreshToken = null;
            await userManager.UpdateAsync(user);
        }
    }
