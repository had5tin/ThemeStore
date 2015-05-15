using System.Collections.Generic;
using Falcon.Data.Domain;

namespace Falcon.Services.Users
{
    public interface IUserService : IService
    {
        User GetUserById(int userId);        
        User GetUserByUsername(string username);
        User GetUserByEmail(string email);

        User GetUserForUpdate(int userId);
        
        int AddUser(User user);
        void UpdateUser(User user);
        void RemoveUser(User user);

        User ValidateUser(string username, string password);

        IEnumerable<User> GetAll();
        IEnumerable<User> GetAll(int pageIndex, int pageSize);
        int GetAllCount();
    }
}
