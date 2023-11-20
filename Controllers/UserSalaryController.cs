using DotnetAPI.data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]

public class UserSalaryController : ControllerBase
{
    DataContextDapper _dapper;

    public UserSalaryController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    /* Get users salary */
    [HttpGet("GetUserSalary")]
    public IEnumerable<UserSalary> GetUserSalaries()
    {
        string sqlQuery = @"
        SELECT 
            [UserId],
            [Salary]
        FROM TutorialAppSchema.UserSalary";

        IEnumerable<UserSalary> userSalaries = _dapper.LoadData<UserSalary>(sqlQuery);

        return userSalaries;
    }

    /* Getting user salary by Id */
    [HttpGet("GetSingleUserSalary/{userId}")]
    public UserSalary GetSingleUserSalary(int userId)
    {
        string sqlQuery = @"
        SELECT 
            [UserId],
            [Salary]
        FROM TutorialAppSchema.UserSalary
        WHERE UserId = " + userId.ToString();

        UserSalary userSalary = _dapper.LoadDataSingle<UserSalary>(sqlQuery);

        return userSalary;
    }

    /* Editting user salary */
    [HttpPut("EditUserSalary")]
    public IActionResult EditUserSalary(UserSalary userSalary) 
    {
        string sqlQuery = @"
        UPDATE TutorialAppSchema.UserSalary
            SET [Salary] = '" + userSalary.Salary + 
            "', [AvgSalary] = '" + userSalary.AvgSalary + 
        "' WHERE UserId = " + userSalary.UserId;

        if (_dapper.ExecuteSql(sqlQuery)) 
        {
            return Ok();
        }

        throw new Exception("Failed to Edit");
    }

    [HttpPost]
    public IActionResult AddUserSalary(UserSalary userSalary) 
    {
        string sqlQuery = @"
            INSERT INTO TutorialAppSchema.UserSalary(
                [UserId],
                [Salary],
                [AvgSalary]
            ) VALUES 
            (" + 
                "'" + userSalary.UserId +
                "','" + userSalary.Salary +
                "','" + userSalary.AvgSalary +
            "')";

        if (_dapper.ExecuteSql(sqlQuery))
        {
            return Ok();
        }

        throw new Exception("Failed to add user salaray");
    }
}