using UserManagementAPI.Models;
using System.Collections.Concurrent;

namespace UserManagementAPI
{
    public class UserService:IUserService
    {
        private readonly ConcurrentDictionary<int, User> _users = new();
        public UserService()
        {
            // Initialize with some sample users
            _users.TryAdd(1, new User
            {
                Id = 1,
                UserName = "JohnDoe",
                Email = "john.doe@example.com"
            });
            _users.TryAdd(2, new User
            {
                Id = 2,
                UserName = "JaneSmith",
                Email = "smith@email.com"
            });
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _users.Values;
        }
        public User GetUser(int id)
        {
            _users.TryGetValue(id, out var user);
            if(user == null)
            {
                throw new ArgumentException("User not found");
            }
            return user;
        }
        public void AddUser(User user)
        {
            if(_users.ContainsKey(user.Id))
            {
                throw new ArgumentException("User with the same ID already exists");
            }
            _users.TryAdd(user.Id, user);
        }

        public void UpdateUser(int id, User user)
        {
            _users.AddOrUpdate(id, user, (key, oldValue) => {
                oldValue.UserName = user.UserName;
                oldValue.Email = user.Email;
                return oldValue;
            });
        }

        public void DeleteUser(int id)
        {
            if(!_users.ContainsKey(id))
            {
                throw new ArgumentException("User not found");
            }
            _users.TryRemove(id, out _);
        }
    }
}
