using Hermes.Data;
using Hermes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Hermes.Dtos.Users;

namespace Hermes.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [AllowAnonymous]
    [HttpGet("get_all")]
    public IEnumerable<UserDto> GetUsers()
    {
        string query = "SELECT * FROM users ORDER BY id";
        IEnumerable<UserDto> users = _dapper.LoadData<UserDto>(query);
        return users;

    }

    [HttpGet("get_one/{id}")]
    public UserModel GetUsers(int id)
    {
        string query = $"SELECT * FROM users WHERE id = {id}";
        UserModel users = _dapper.LoadDataSingle<UserModel>(query);
        return users;
    }

    [HttpPut("edit/{id}")]
    public IActionResult EditUser([FromBody] UserDto user, int id)
    {
        string query = $"UPDATE users SET email = '{user.Email}' WHERE id = {id}";
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }


    [HttpDelete("delete/{id}")]
    public IActionResult DeleteUser(int id)
    {
        string query = $"UPDATE users SET is_active = false WHERE id = {id}";
        if (_dapper.ExecuteSql(query))
        {
            return Ok();
        }

        throw new Exception("Failed to delete user");
    }
}
