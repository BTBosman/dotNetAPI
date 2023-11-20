using AutoMapper;
using DotnetAPI.data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class UserEFController : ControllerBase
{
    DataContextEF _entityFramework;
    IMapper _mapper;

    public UserEFController(IConfiguration config)
    {
        _entityFramework = new DataContextEF(config);
        _mapper = new Mapper(new MapperConfiguration(config => {
            config.CreateMap<UserToAddDto, User>();
        }));
    }

    /* Getting all users in the Users table Descending by ID */
    [HttpGet("GetUsers")]

    public IEnumerable<User> GetUsers()
    {
        IEnumerable<User> users  = _entityFramework.Users.ToList<User>();
        return users;
    }

/* Getting a single user by ID */
[HttpGet("GetSingleUser/{userId}")]
    public User GetSingleUser(int userId)
    {      
        User? user  = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();
        
        if (user != null)
        {

            return user;
        }

        throw new Exception("Unable to find user");
    }

/* Get user by status */
[HttpGet("GetActiveUsers/{status}")]
public IEnumerable<User> ActiveUsers(bool status)
{
    IEnumerable<User> users = _entityFramework.Users.ToList<User>().Where<User>(u => u.Active == status);

    return users;
}

/* Edit User data by ID */
[HttpPut("EditUser")]
public IActionResult EditUser(User user)
{   
    User? userDb  = _entityFramework.Users
                .Where(u => u.UserId == user.UserId)
                .FirstOrDefault<User>();
        
    if (userDb != null)
    {
        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        if( _entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to update user.");
    }

    throw new Exception("Unable to find user to update.");
}

/* Adding a user to the user table. */
[HttpPost("AddUser")]
public IActionResult AddUser(UserToAddDto user)
{
        User userDb =  _mapper.Map<User>(user);
        
        _entityFramework.Add(userDb);

    if( _entityFramework.SaveChanges() > 0)
    {
        return Ok();
    }

    throw new Exception("Failed to add user");
   
}

[HttpDelete("DeleteUser/{userId}")]
public IActionResult DeleteUser(int userId)
{
    User? userDb  = _entityFramework.Users
                .Where(u => u.UserId == userId)
                .FirstOrDefault<User>();
        
    if (userDb != null)
    {
        _entityFramework.Users.Remove(userDb);
        if( _entityFramework.SaveChanges() > 0)
        {
            return Ok();
        }

        throw new Exception("Failed to delete user.");
    }

    throw new Exception("Unable to find user to delete user.");
}

}
