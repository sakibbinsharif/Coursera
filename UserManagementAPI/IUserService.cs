using UserManagementAPI.Models;

namespace UserManagementAPI
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUser(int id);
        void AddUser(User user);
        void UpdateUser(int id, User user);
        void DeleteUser(int id);
    }
}
