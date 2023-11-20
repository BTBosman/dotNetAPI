using DotnetAPI.data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    DataContextDapper _dapper;

    public UserController(IConfiguration config)
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
    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        string sqlQuery = @"
            SELECT 
                [UserId],
                [FirstName], 
                [LastName],
                [Active],
                [Gender],
                [Email]
            FROM TutorialAppSchema.Users
            ORDER BY UserId DESC";
        IEnumerable<User> users  = _dapper.LoadData<User>(sqlQuery);

        return users;
    }

/* Getting a single user by ID */
[HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {
        string sqlQuery = @"
            SELECT 
                [UserId],
                [FirstName], 
                [LastName],
                [Active],
                [Gender],
                [Email]
            FROM TutorialAppSchema.Users
            WHERE UserId = " + userId.ToString();
        
        User user  = _dapper.LoadDataSingle<User>(sqlQuery);
        return user;
    }

/* Get user by status */
[HttpGet("GetActiveUsers/{status}")]
public IEnumerable<User> ActiveUsers(int status)
{
    string sqlQuery = @"
            SELECT 
                [UserId],
                [FirstName], 
                [LastName],
                [Active],
                [Gender],
                [Email]
            FROM TutorialAppSchema.Users
            WHERE Active = " + status.ToString();
    IEnumerable<User> usersByStatus = _dapper.LoadData<User>(sqlQuery);

    return usersByStatus;
}

/* Edit User data by ID */
[HttpPut("EditUser")]
public IActionResult EditUser(User user)
{
    string sqlQuery = @"
    UPDATE TutorialAppSchema.Users
    SET [FirstName] = '" + user.FirstName + 
        "', [LastName] = '" + user.LastName + 
        "', [Email] = '" + user.Email + 
        "', [Gender] = '" + user.Gender + 
        "', [Active] = '" + user.Active +
    "' WHERE UserId = " + user.UserId;
    
    if( _dapper.ExecuteSql(sqlQuery))
    {
        return Ok();
    }

    throw new Exception("Failed to update user");
}

/* Adding a user to the user table. */
[HttpPost("AddUser")]
public IActionResult AddUser(UserToAddDto user)
{
    string sqlQuery = @"
        INSERT INTO TutorialAppSchema.Users(
            [FirstName],
            [LastName],
            [Email],
            [Gender],
            [Active]
        )
        VALUES 
        (" + 
            "'" + user.FirstName + 
            "','" + user.LastName + 
            "','" + user.Email + 
            "','" + user.Gender + 
            "','" + user.Active +
        "')";

    if (_dapper.ExecuteSql(sqlQuery)) {

        return Ok();
    }
    
    throw new Exception("Failed to add user");
}

[HttpDelete("DeleteUser/{userId}")]
public IActionResult DeleteUser(int userId)
{
    string sqlQuery = @"
        DELETE FROM TutorialAppSchema.Users
            WHERE UserId = " + userId.ToString();
    if(_dapper.ExecuteSql(sqlQuery))
    {

        return Ok();
    }

    Console.WriteLine("Delete Sql: " + sqlQuery);

    throw new Exception("Failed to delete user");
}

}
