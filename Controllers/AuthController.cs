using Hermes.Data;
using static BCrypt.Net.BCrypt;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hermes.Helpers;
using Hermes.Dtos.Users;
using Hermes.Models;

namespace Hermes.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly AuthHelper _authHelper;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel userRegistration)
        {
                string query = $"SELECT email FROM users WHERE email = '{userRegistration.Email}'";
                IEnumerable<string> ExistingUsers = _dapper.LoadData<string>(query);

                if(ExistingUsers.Count() == 0)
                {
                    var hashedPassword = HashPassword(userRegistration.Password);

                    if (_dapper.ExecuteSql($"INSERT INTO users (name, email, password, salt, active, admin, jwt, intranet_id, dpo, setor) VALUES('{userRegistration.Name}', '{userRegistration.Email}', '{hashedPassword}', 10, 1, {userRegistration.Admin}, 0, 0, 0, '{userRegistration.Setor}')"))
                    {
                        return Ok();
                    }

                    return StatusCode(500, "Error in registering user");
                }
                return StatusCode(401, "This user already exists");
        }


        [AllowAnonymous]
        [HttpPost("login")]
        public IActionResult Login([FromBody]UserForLoginDto userForLoginDto)
        {
            string query = $"SELECT * FROM users WHERE email = '{userForLoginDto.Email}' and active = 1";
            IEnumerable<UserForLoginDto> ExistingUser = _dapper.LoadData<UserForLoginDto>(query);

            if (ExistingUser.Count() == 1)
            {

                UserForLoginDto passwordDb = _dapper.LoadDataSingle<UserForLoginDto>(query);

                if(Verify(userForLoginDto.Password, passwordDb.Password))
                {
                    string userIdSql = $"SELECT id FROM users WHERE email = '{userForLoginDto.Email}'";
                    int userId = _dapper.LoadDataSingle<int>(userIdSql);
                    return Ok(new Dictionary<string, string>
                    {
                        {"token", _authHelper.CreateToken(userId)}
                    });
                }
                return StatusCode(401, "Incorrect password!");
            }
            return StatusCode(404, "User not found or inactive.");
        }

        [HttpGet("RefreshToken")]
        public string RefreshToken()
        {
            string userIdSql = $"SELECT id FROM users WHERE id = '{User.FindFirst("userId")?.Value}'";

            int userId = _dapper.LoadDataSingle<int>(userIdSql);
            return _authHelper.CreateToken(userId);
        }
    }
}
