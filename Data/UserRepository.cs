using DotnetAPI.Models;

namespace DotnetAPI.data
{
    public class UserRepository : IUserRepository
    {
        DataContextEF _entityFramework;
        public UserRepository(IConfiguration config)
        {
            _entityFramework = new DataContextEF(config);
        }

        public bool SaveChanges()
        {
            return _entityFramework.SaveChanges() > 0;
        }

        public void AddEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null) 
            {
                _entityFramework.Add(entityToAdd);
            }
        }

        public void RemoveEntity<T>(T entityToAdd)
        {
            if (entityToAdd != null) 
            {
                _entityFramework.Remove(entityToAdd);
            }
        }

        public IEnumerable<User> GetUsers()
        {
            IEnumerable<User> users  = _entityFramework.Users.ToList<User>();
            return users;
        }

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

        public IEnumerable<User> GetActiveUsers(bool status)
        {
            IEnumerable<User> users = _entityFramework.Users.ToList<User>().Where<User>(u => u.Active == status);

            return users;
        }

        public UserSalary GetSingleUserSalary(int userId)
        {      
            UserSalary? userSalary  = _entityFramework.UserSalary
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault<UserSalary>();
            
            if (userSalary != null)
            {

                return userSalary;
            }

            throw new Exception("Unable to find user");
        }

        public UserJobInfo GetSingleUserJobInfo(int userId)
        {      
            UserJobInfo? userJobInfo  = _entityFramework.UserJobInfo
                    .Where(u => u.UserId == userId)
                    .FirstOrDefault<UserJobInfo>();
            
            if (userJobInfo != null)
            {

                return userJobInfo;
            }

            throw new Exception("Unable to find user");
        }
    }
}