using System;
using System.Linq;
using System.Web;

using Falcon;
using Falcon.Security;
using Falcon.Data.Domain;
using Falcon.Services.Users;
//using Falcon.Services.Accounts;
using Falcon.Infrastructure;

namespace Falcon.Services
{
    /// <summary>
    /// Working context
    /// </summary>
    public partial class WorkContext : IWorkContext
    {
        
        private readonly IAuthenticationService _authenService;
        private readonly IWebHelper _webHelper;

        private HttpContext _httpContext;
        
        private const string UserCookieName = "falcon";
        private bool _cachedIsAdmin = false;
        private User _cachedUser;
        //private Account _cachedAccount;

        public WorkContext(IAuthenticationService authenService, IWebHelper webHelper)
        {
            _authenService = authenService;
            _webHelper = webHelper;
            _httpContext = HttpContext.Current;
        }

        protected User GetCurrentUser()
        {
            if (_cachedUser != null)
                return _cachedUser;

            if (_httpContext != null)
            {               
                //registered user
                _cachedUser = _authenService.GetAuthenticatedUser();
            }

            //return ObjectCopier.Clone(_cachedUser);
            return _cachedUser;
        }

        //protected Account GetCurrentAccount()
        //{
        //    if (_cachedAccount != null)
        //        return _cachedAccount;

        //    Account account = null;
        //    if (_httpContext != null)
        //    {
        //        //check whether request is made by a search engine
        //        //in this case return built-in Account record for search engines 
        //        //or comment the following two lines of code in order to disable this functionality
        //        //if (_webHelper.IsSearchEngine(_httpContext.Request))
        //        //    Account = _AccountService.GetAccountByAccountname("__SearchEngine");

        //        //registered Account
        //        account = _authenService.GetAuthenticatedAccount();

        //        _cachedAccount = account;
        //    }

        //    //return ObjectCopier.Clone(_cachedAccount);
        //    return _cachedAccount;
        //}

        /// <summary>
        /// Gets the current user
        /// </summary>
        public User CurrentUser
        {
            get
            {
                return GetCurrentUser();
            }           
        }

        /// <summary>
        /// Trả về Id của User Admin đăng nhập hiện tại.
        /// Nếu không tồn tại thì trả về -1
        /// </summary>
        public int CurrentUserId
        {
            get
            {
                return _authenService.GetAuthenticatedUserId();
            }
        }

        //public Account CurrentAccount
        //{
        //    get
        //    {
        //        return GetCurrentAccount();
        //    }
        //}

        /// <summary>
        /// Trả về Id của Account đăng nhập hiện tại.
        /// Nếu không tồn tại thì trả về -1
        /// </summary>
        //public int CurrentAccountId
        //{
        //    get
        //    {
        //        return _authenService.GetAuthenticatedAccountId();
        //    }
        //}

        public bool IsAdmin
        {
            get
            {
                return _cachedIsAdmin;
            }
            set
            {
                _cachedIsAdmin = value;
            }
        }
    }
}
