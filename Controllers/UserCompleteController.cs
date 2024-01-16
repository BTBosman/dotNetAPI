using DotnetAPI.data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserCompleteController : ControllerBase
{
    DataContextDapper _dapper;

    public UserCompleteController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    /*  Database connection test. */
    [HttpGet("TestConnection")]
    public DateTime TestConnection()
    {
        return _dapper.LoadDataSingle<DateTime>("SELECT GETDATE()");
    }

    /* Getting all users in the Users table Descending by ID */
    [HttpGet("GetUsers/{userId}/{isActive}")]
    public IEnumerable<UserComplete> GetUsers(int userId, bool isActive)
    {
        string sqlQuery = @"EXEC TutorialAppSchema.spUsers_Get";
        string parameters = "";
        if (userId != 0)
        {
            parameters += ", @UserId =" + userId.ToString();
        }

        if (isActive)
        {
            parameters += ", @Active =" + isActive.ToString();
        }

        sqlQuery += parameters.Substring(1); //parameters.Length);
        IEnumerable<UserComplete> users = _dapper.LoadData<UserComplete>(sqlQuery);

        return users;
    }


    /* Edit User data by ID */
    [HttpPut("UpsertUser")]
    public IActionResult UpsertUser(UserComplete user)
    {
        string sqlQuery = @"EXEC TutorialAppSchema.spUser_Upsert 
        @FirstName = '" + user.FirstName +
            "', @LastName = '" + user.LastName +
            "', @Email = '" + user.Email +
            "', @Gender = '" + user.Gender +
            "', @Active = '" + user.Active +
            "', @JobTitle = '" + user.JobTitle +
            "', @Department = '" + user.Department +
            "', @Salary = '" + user.Salary +
            "',  @UserId = " + user.UserId;

        Console.WriteLine("-----" + sqlQuery);

        if (_dapper.ExecuteSql(sqlQuery))
        {
            return Ok();
        }

        throw new Exception("Failed to update user");
    }

    [HttpDelete("DeleteUser/{userId}")]
    public IActionResult DeleteUser(int userId)
    {
        string sqlQuery = @"
        EXEC TutorialAppSchema.spUser_Delete
            @UserId = " + userId.ToString();
        if (_dapper.ExecuteSql(sqlQuery))
        {

            return Ok();
        }

        Console.WriteLine("Delete Sql: " + sqlQuery);

        throw new Exception("Failed to delete user");
    }

}
