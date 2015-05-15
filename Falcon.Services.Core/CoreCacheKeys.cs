using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Themes;
using Falcon.Data.Domain;

namespace Falcon.Services
{
    public class CoreCacheKeys
    {
        public static class ThemeKeys
        {
            public static string ThemeByType(ThemeType type)
            {
                return "ThemeType." + type.ToString();
            }
        }
        
        public static class SystemSettingKeys
        {
            public static string SystemSettingByKey(string key)
            {
                return "SystemSetting." + key;
            }

            public static string SystemSettingPattern()
            {
                return @"^SystemSetting\..$";
            }
        }

        //public static class AccountKeys
        //{
        //    public static string AccountById(int id)
        //    {
        //        return "Account.ById." + id;
        //    }

        //    public static string AccountByUsernameOrEmail(string usernameOrEmail)
        //    {
        //        return "Account.ByUsernameOrEmail." + usernameOrEmail;
        //    }

        //    public static string[] AccountCacheKeys(Account account)
        //    {
        //        return new string[] {
        //            AccountById(account.Id),
        //            AccountByUsernameOrEmail(account.UserName),
        //            AccountByUsernameOrEmail(account.Email),
        //        };
        //    }

        //    public static string AccountPattern()
        //    {
        //        return @"^Account\..$";
        //    }
        //}

        public static class UserKeys
        {
            public static string UserById(int id)
            {
                return "User.ById." + id;
            }

            public static string UserByUsernameOrEmail(string usernameOrEmail)
            {
                return "User.ByUsernameOrEmail." + usernameOrEmail;
            }

            public static string[] UserCacheKeys(User user)
            {
                return new string[] {
                    UserById(user.Id),
                    UserByUsernameOrEmail(user.UserName),
                    UserByUsernameOrEmail(user.Email),
                };
            }

            public static string UserPattern()
            {
                return @"^User\..$";
            }
        }
    }
}
