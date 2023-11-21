using DotnetAPI.Models;

namespace DotnetAPI.data 
{
    public interface IUserRepository
    {
        public bool SaveChanges();
        public void AddEntity<T>(T entityToAdd);
        public void RemoveEntity<T>(T entityToAdd);
        public IEnumerable<User> GetUsers();
        public User GetSingleUser(int userId);
        public IEnumerable<User> GetActiveUsers(bool status);
        public UserSalary GetSingleUserSalary(int userId);
        public UserJobInfo GetSingleUserJobInfo(int userId);

    }
}