/// <copyright file="FormsAuthenticationService.cs" company="DKT">
/// Copyright (c) 2012 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Web.Security;
using System.Web;

//using Falcon.Logging;
using Falcon.Data.Domain;
using Falcon.Services.Users;
using Falcon.Security;
//using Falcon.Services.Accounts;

namespace Falcon.Services.Security
{
    public class FormsAuthenticationService : IAuthenticationService
    {
        private User _signedInUser;
        //private Account _signedInAccount;
        private IUserService _userService;

        public FormsAuthenticationService(IUserService userService)
        {
            _userService = userService;

            ExpirationTimeSpan = TimeSpan.FromMinutes(60);
            AccountExpirationTimeSpan = TimeSpan.FromDays(30);
        }

        //public ILogger Logger { get; set; }

        public TimeSpan ExpirationTimeSpan { get; set; }
        public TimeSpan AccountExpirationTimeSpan { get; set; }

        #region System user
        public void SignIn(User user, bool createPersistentCookie)
        {
            var now = DateTime.UtcNow.ToLocalTime();
            var userData = "sysuser-" + user.Id;

            var ticket = new FormsAuthenticationTicket(
                1 /*version*/,
                user.Email,
                now,
                now.Add(ExpirationTimeSpan),
                createPersistentCookie,
                userData,
                FormsAuthentication.FormsCookiePath);

            var encryptedTicket = FormsAuthentication.Encrypt(ticket);

            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }

            if (createPersistentCookie)
            {
                cookie.Expires = ticket.Expiration;
            }

            var httpContext = HttpContext.Current;
            httpContext.Response.Cookies.Add(cookie);
            _signedInUser = user;
        }

        public void SetAuthenticatedUserForRequest(User user)
        {
            _signedInUser = user;
        }

        public User GetAuthenticatedUser(FormsIdentity identity)
        {
            var userData = identity.Ticket.UserData;

            if (userData.StartsWith("account-"))
            {
                return null;
            }
            userData = userData.Substring(8);

            int userId;
            if (!int.TryParse(userData, out userId))
            {
                //Logger.Fatal("User id not a parsable integer");
                return null;
            }
            return _userService.GetUserById(userId);
        }

        public User GetAuthenticatedUser()
        {
            if (_signedInUser != null)
                return _signedInUser;

            var httpContext = HttpContext.Current;
            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }

            var formsIdentity = (FormsIdentity)httpContext.User.Identity;
            return GetAuthenticatedUser(formsIdentity);
        }
        #endregion

        #region Account
        //public void AccountSignIn(Account account, bool createPersistentCookie)
        //{
        //    var now = DateTime.UtcNow.ToLocalTime();
        //    var userData = "account-" + account.Id;

        //    var ticket = new FormsAuthenticationTicket(
        //        1 /*version*/,
        //        account.Email,
        //        now,
        //        now.Add(AccountExpirationTimeSpan),
        //        createPersistentCookie,
        //        userData,
        //        FormsAuthentication.FormsCookiePath);

        //    var encryptedTicket = FormsAuthentication.Encrypt(ticket);

        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
        //    cookie.HttpOnly = true;
        //    cookie.Secure = FormsAuthentication.RequireSSL;
        //    cookie.Path = FormsAuthentication.FormsCookiePath;
            
        //    if (FormsAuthentication.CookieDomain != null)
        //    {
        //        cookie.Domain = FormsAuthentication.CookieDomain;
        //    }

        //    if (createPersistentCookie)
        //    {
        //        cookie.Expires = ticket.Expiration;
        //    }

        //    var httpContext = HttpContext.Current;
        //    httpContext.Response.Cookies.Add(cookie);            
        //    _signedInAccount = account;
        //}

        //public void SetAuthenticatedAccountForRequest(Account account)
        //{
        //    _signedInAccount = account;
        //}

        //public Account GetAuthenticatedAccount(FormsIdentity identity)
        //{
        //    var userData = identity.Ticket.UserData;
        //    if (userData.StartsWith("sysuser-"))
        //    {
        //        //Check if using Fake Account
        //        var httpContext = HttpContext.Current;
        //        if (httpContext != null && httpContext.Request.IsAuthenticated && (httpContext.User.Identity is FormsIdentity))
        //        {
        //            if (httpContext.Session["__FakeAccountId"] != null) 
        //            {
        //                int fakeAccountId = -1;
        //                if (!int.TryParse(httpContext.Session["__FakeAccountId"].ToString(), out fakeAccountId))
        //                {
        //                    Logger.Fatal("FakeAccountId not a parsable integer");
        //                    return null;
        //                }
        //                return _accountService.GetAccountById(fakeAccountId);
        //            }
        //        }
        //        return null;
        //    }
        //    userData = userData.Substring(8);
        //    int accountId;
        //    if (!int.TryParse(userData, out accountId))
        //    {
        //        Logger.Fatal("User id not a parsable integer");
        //        return null;
        //    }
        //    return _accountService.GetAccountById(accountId);
        //}

        //public Account GetAuthenticatedAccount()
        //{
        //    if (_signedInAccount != null)
        //        return _signedInAccount;

        //    var httpContext = HttpContext.Current;
        //    if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
        //    {
        //        return null;
        //    }

        //    var formsIdentity = (FormsIdentity)httpContext.User.Identity;
        //    _signedInAccount = GetAuthenticatedAccount(formsIdentity);
        //    return _signedInAccount;
        //}
        #endregion

        public void SignOut()
        {
            _signedInUser = null;
            //_signedInAccount = null;
            FormsAuthentication.SignOut();
        }


        public int GetAuthenticatedUserId()
        {
            if (_signedInUser != null)
                return _signedInUser.Id;

            var httpContext = HttpContext.Current;
            if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
            {
                return -1;
            }

            var formsIdentity = (FormsIdentity)httpContext.User.Identity;
            var userData = formsIdentity.Ticket.UserData;
            if (userData.StartsWith("account-"))
            {
                return -1;
            }
            userData = userData.Substring(8);
            int userId;
            if (!int.TryParse(userData, out userId))
            {
                //Logger.Fatal("User id not a parsable integer");
                return -1;
            }
            return userId;
        }

        //public int GetAuthenticatedAccountId()
        //{
        //    if (_signedInAccount != null)
        //        return _signedInAccount.Id;

        //    var httpContext = HttpContext.Current;
        //    if (httpContext == null || !httpContext.Request.IsAuthenticated || !(httpContext.User.Identity is FormsIdentity))
        //    {
        //        return -1;
        //    }

        //    var formsIdentity = (FormsIdentity)httpContext.User.Identity;
        //    var userData = formsIdentity.Ticket.UserData;
        //    if (userData.StartsWith("sysuser-"))
        //    {
        //        //Check if using Fake Account
        //        if (httpContext.Session["__FakeAccountId"] != null)
        //        {
        //            int fakeAccountId = -1;
        //            if (!int.TryParse(httpContext.Session["__FakeAccountId"].ToString(), out fakeAccountId))
        //            {
        //                Logger.Fatal("FakeAccountId not a parsable integer");
        //                return -1;
        //            }
        //            return fakeAccountId;
        //        }
        //        return -1;
        //    }
        //    userData = userData.Substring(8);
        //    int accountId;
        //    if (!int.TryParse(userData, out accountId))
        //    {
        //        Logger.Fatal("AccountId not a parsable integer");
        //        return -1;
        //    }
        //    return accountId;
        //}


        //public void FakeAccountSignIn(Account account)
        //{
        //    var httpContext = HttpContext.Current;
        //    if (httpContext != null && httpContext.Request.IsAuthenticated && (httpContext.User.Identity is FormsIdentity))
        //    {
        //        httpContext.Session["__FakeAccountId"] = account.Id;
        //    }
        //}
    }
}
