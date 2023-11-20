using DotnetAPI.data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserJobInfoController : ControllerBase
{
    DataContextDapper _dapper;

    public UserJobInfoController(IConfiguration config)
    {
        _dapper = new DataContextDapper(config);
    }

    /* Getting all user job info */
    [HttpGet("getUsersJobInfo")]
    public IEnumerable<UserJobInfo> GetUserJobInfo()
    {
        string sqlQuery = @"
        SELECT
            [UserId],
            [JobTitle],
            [Department]
        FROM TutorialAppSchema.UserJobInfo
        ORDER BY UserId DESC";

        IEnumerable<UserJobInfo> usersJobInfo = _dapper.LoadData<UserJobInfo>(sqlQuery);
        return usersJobInfo;
    }

    /* Getting a single user job info */
    [HttpGet("GetSingleUserJobInfo/{userId}")]
    public UserJobInfo GetSingleUserJobInfo(int userId)
    {
        string sqlQuery = @"
        SELECT
            [UserId],
            [JobTitle],
            [Department]
        FROM TutorialAppSchema.UserJobInfo
        WHERE UserId = " + userId.ToString();

        UserJobInfo userJobInfo = _dapper.LoadDataSingle<UserJobInfo>(sqlQuery);
        return userJobInfo;
    }

    /* Edit User Job Info */
    [HttpPut("EditUserJobInfo")]
    public IActionResult EditUserJobInfo(UserJobInfo userJobInfo)
    {
        string sqlQuery = @"
        UPDATE TutorialAppSchema.UserJobInfo
        SET 
        [JobTitle] = '" + userJobInfo.JobTitle + 
        "', [Department] = '" + userJobInfo.Department +
        "' WHERE UserId = " + userJobInfo.UserId;

        if (_dapper.ExecuteSql(sqlQuery))
        {
            return Ok();
        }

        throw new Exception("Failed To Update User Job Info");
    }

    /* Adding User Job Info */
    [HttpPost("AddUserJobInfo")]
    public IActionResult AddUserJobInfo(UserJobInfoToAddDto userJobInfo)
    {
        string sqlQuery = @"
        INSERT INTO TutorialAppSchema.UserJobInfo(
        [UserId],
        [JobTitle],
        [Department]
    ) 
    VALUES 
    (" +
        "'" + userJobInfo.UserId +
        "','" + userJobInfo.JobTitle +
        "','" + userJobInfo.Department +
    "')";
    
    if (_dapper.ExecuteSql(sqlQuery))
    {
        return Ok();
    }

    throw new Exception("Unable to Add user Job Info");
    }
}