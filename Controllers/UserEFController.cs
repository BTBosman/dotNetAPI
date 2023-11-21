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
    IUserRepository _userRepository;
    IMapper _mapper;

    public UserEFController(IConfiguration config, IUserRepository userRepository)
    {
        _userRepository = userRepository;
        
        _mapper = new Mapper(new MapperConfiguration(config => {
            config.CreateMap<UserToAddDto, User>();
        }));
    }

    /* Getting all users in the Users table Descending by ID */
    [HttpGet("GetUsers")]
    public IEnumerable<User> GetUsers() 
    {
        IEnumerable<User> users  = _userRepository.GetUsers();
        return users;
    }

/* Getting a single user by ID */
[HttpGet("GetSingleUser/{userId}")]
public User GetSingleUser(int userId)
{      
    return _userRepository.GetSingleUser(userId);
}

/* Get user by status */
[HttpGet("GetActiveUsers/{status}")]
public IEnumerable<User> ActiveUsers(bool status)
{
    return _userRepository.GetActiveUsers(status);
}

/* Edit User data by ID */
[HttpPut("EditUser")]
public IActionResult EditUser(User user)
{   
    User? userDb  = _userRepository.GetSingleUser(user.UserId);
        
    if (userDb != null)
    {
        userDb.Active = user.Active;
        userDb.FirstName = user.FirstName;
        userDb.LastName = user.LastName;
        userDb.Email = user.Email;
        userDb.Gender = user.Gender;
        if (_userRepository.SaveChanges())
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
        
        _userRepository.AddEntity<User>(userDb);

    if (_userRepository.SaveChanges())
    {
        return Ok();
    }

    throw new Exception("Failed to add user");
   
}

[HttpDelete("DeleteUser/{userId}")]
public IActionResult DeleteUser(int userId)
{
    User? userDb  = _userRepository.GetSingleUser(userId);
        
    if (userDb != null)
    {
        _userRepository.RemoveEntity<User>(userDb);
        if (_userRepository.SaveChanges())
        {
            return Ok();
        }

        throw new Exception("Failed to delete user.");
    }

    throw new Exception("Unable to find user to delete user.");
}

}
