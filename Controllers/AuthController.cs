using Hermes.Data;
using Hermes.Dtos;
using static BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Hermes.Models;

namespace Hermes.Controllers
{
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserForRegistrationDto userForRegistrationDto)
        {
            if(userForRegistrationDto.Password == userForRegistrationDto.PasswordConfirm)
            {
                string query = $"SELECT email FROM users WHERE email = '{userForRegistrationDto.Email}'";
                IEnumerable<string> ExistingUsers = _dapper.LoadData<string>(query);

                if(ExistingUsers.Count() == 0)
                {
                    var hashedPassword = HashPassword(userForRegistrationDto.Password);

                    if (_dapper.ExecuteSql($"INSERT INTO users (name, email, password) VALUES('{userForRegistrationDto.Name}', '{userForRegistrationDto.Email}', '{hashedPassword}')"))
                    {
                        return Ok();
                    }

                    throw new Exception("Error in registering user");
                }
                throw new Exception("This user already exists");
            }
            throw new Exception("Passwords do not match");
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]UserForLoginDto userForLoginDto)
        {

            string query = $"SELECT * FROM users WHERE email = '{userForLoginDto.Email}'";
            IEnumerable<string> ExistingUser = _dapper.LoadData<string>(query);


            if (ExistingUser.Count() == 1)
            {

                UserForLoginDto passwordDb = _dapper.LoadDataSingle<UserForLoginDto>(query);

                if(Verify(userForLoginDto.Password, passwordDb.Password))
                {
                    string userIdSql = $"SELECT id FROM users WHERE email = '{userForLoginDto.Email}'";
                    int userId = _dapper.LoadDataSingle<int>(userIdSql);
                    return Ok(new Dictionary<string, string>
                    {
                        {"token", CreateToken(userId)}
                    });
                }
                return StatusCode(401, "Incorrect password!");
            }
            throw new Exception("User does not exist");
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string userIdSql = $"SELECT id FROM users WHERE id = '{User.FindFirst("userId")?.Value}'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);
            return CreateToken(userId);
        }

        private string CreateToken(int userId)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("userId", userId.ToString())
            };

            string? tokenKeyString = _config.GetSection("AppSettings:TokenKey").Value;
            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyString != null ? tokenKeyString : ""));

            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                SigningCredentials = credentials,
                Expires = DateTime.Now.AddHours(8),
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityToken token = tokenHandler.CreateToken(descriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
