using Hermes.Data;
using Hermes.Dtos;
using Hermes.Models;
using Microsoft.AspNetCore.Mvc;

namespace Hermes.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    [HttpGet("get_all")]
    public IEnumerable<UserModel> GetUsers()
    {
        string query = "SELECT * FROM users ORDER BY id";
        IEnumerable<UserModel> users = _dapper.LoadData<UserModel>(query);
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
        string query = $"UPDATE users SET email = '{user.Email}', age = {user.Age} WHERE id = {id}";
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
