using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Falcon.Data.Domain;
using Falcon.Data.Repository;

using Dapper;
using Falcon.Caching;
using System.Security.Cryptography;

namespace Falcon.Services.Users
{
    public class UserService : BaseService, IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }
      
        public User GetUserById(int userId)
        {
            return AspectF.Define
                .Cache<User>(Cache, CoreCacheKeys.UserKeys.UserById(userId), 3600)
                .Return<User>(() => _userRepository.Table.SingleOrDefault(u => u.Id == userId));
        }

        public User GetUserByUsername(string username)
        {
            return AspectF.Define
                .Cache<User>(Cache, CoreCacheKeys.UserKeys.UserByUsernameOrEmail(username), 3600)
                .Return<User>(() => _userRepository.Table.FirstOrDefault(u => u.UserName == username));
        }

        public User GetUserByEmail(string email)
        {
            return AspectF.Define
                .Cache<User>(Cache, CoreCacheKeys.UserKeys.UserByUsernameOrEmail(email), 3600)
                .Return<User>(() => _userRepository.Table.FirstOrDefault(u => u.Email == email));
        }

        public User GetUserForUpdate(int userId)
        {
            return _userRepository.Table.FirstOrDefault(u => u.Id == userId);
        }

        public int AddUser(User user)
        {
            _userRepository.Add(user);
            return user.Id;
        }

        public void UpdateUser(User user)
        {
            _userRepository.SubmitChanges();
            RemoveUserFromCache(user);
        }

        public void RemoveUser(User user)
        {
            _userRepository.Remove(user);
            RemoveUserFromCache(user);
        }

        public User ValidateUser(string username, string password)
        {
            var user = GetUserByUsername(username);
            if (user != null && user.Password == ComputeMD5Hash(password + user.PasswordSalt))
            {
                return user;
            }
            else
            {
                return null;
            }
        }
        
        public IEnumerable<User> GetAll()
        {
            return _userRepository.Table.ToList<User>();
        }


        public IEnumerable<User> GetAll(int pageIndex, int pageSize)
        {
            return _userRepository.Table.OrderByDescending(b => b.Id).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList<User>();
        }

        public int GetAllCount()
        {
           return _userRepository.Table.Count();
        }

        private void RemoveUserFromCache(User user)
        {
            foreach (string cacheKey in CoreCacheKeys.UserKeys.UserCacheKeys(user))
            {
                Cache.Remove(cacheKey);
            }
        }

        /// <summary>
        /// Tạo chuỗi MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private static string ComputeMD5Hash(string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] data = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
                StringBuilder sBuilder = new StringBuilder();
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }
                return sBuilder.ToString();
            }
        }

    }
}
