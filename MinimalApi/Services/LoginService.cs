using Microsoft.IdentityModel.Tokens;
using MinimalApiDemo.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MinimalApiDemo.Services
{
    public class LoginService : ILoginInService
	{
		public IResult Login(Login user)
		{
			if (user is null)
			{
				return Results.BadRequest("Invalid client request");
			}

			if (user.UserName == "foo" && user.Password == "bar")
			{
				var tokenString = CreateToken();

				return Results.Ok(new AuthenticatedResponse { Token = tokenString });
            }

			return Results.Unauthorized();
		}

		private string CreateToken()
		{
			var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("superSecretKey@345"));

            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                issuer: "https://localhost:7163",
                audience: "https://localhost:7163",
                claims: new List<Claim>(),
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(tokeOptions);		
        }
    }
}

