using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MenuWebapi.Models.Auth;
using MenuWebapi.Models.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
namespace MenuWebapi.Services
{
    public class TokenService
    {
        private readonly JwtOptions jwtOptions;
        public TokenService(
            IOptions<JwtOptions> _jwtOptions
        )
        {
            this.jwtOptions = _jwtOptions.Value;
        }
        public string CreateToken(ApplicationUser user, IList<Claim> userClaims)
        {
            var issuer = jwtOptions.ValidIssuer;
            var audience = jwtOptions.ValidAudience;
            var key = Encoding.ASCII.GetBytes
            (jwtOptions.Key!);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(CreateClaims(user, userClaims)),
                Expires = DateTime.UtcNow.AddDays(10),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials
                (new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            return tokenHandler.WriteToken(token);
        }
        private List<Claim> CreateClaims(ApplicationUser user, IList<Claim> userClaims)
        {
            try
            {
                var role = userClaims.First(w => w.Type == ClaimTypes.Role);
                var claims = new List<Claim>
                                {
                    new Claim(JwtRegisteredClaimNames.Sub, "BackendApi"),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(CultureInfo.InvariantCulture)),
                    new Claim("UserId", user.Id),
                    new Claim(ClaimTypes.Role, role != null ? role.Value : "user")
                                };
                return claims;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}
