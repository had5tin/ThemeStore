using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.Common
{
    public class EmailTemplateConst
    {
        // Account
        public const string ACCOUNT_REGISTER_VERIFY = "ACCOUNT_REGISTER_VERIFY";
        public const string ACCOUNT_CHANGE_PASSWORD = "ACCOUNT_CHANGE_PASSWORD";
        public const string ACCOUNT_RESET_PASSWORD = "ACCOUNT_RESET_PASSWORD";
        public const string ACCOUNT_VERIFIED_SUCCESS = "ACCOUNT_VERIFIED_SUCCESS";
        public const string ACCOUNT_LOCKED = "ACCOUNT_LOCKED";

        // Product
        public const string PRODUCT_APPROVED = "PRODUCT_APPROVED";
        public const string PRODUCT_REJECTED = "PRODUCT_REJECTED";
        public const string PRODUCT_EXPIRED = "PRODUCT_EXPIRED";
        public const string PRODUCT_LOCKED = "PRODUCT_LOCKED";
        public const string PRODUCT_UNLOCKED = "PRODUCT_UNLOCKED";
        public const string PRODUCT_DELETED = "PRODUCT_DELETED";
        public const string PRODUCT_EXISTS = "PRODUCT_EXISTS";

        public const string CONTACT_FROM_ENDUSER = "CONTACT_FROM_ENDUSER";

        public const string STORE_VERIFIED_SUCCESS = "STORE_VERIFIED_SUCCESS";
        public const string STORE_LOCKED = "STORE_LOCKED";
        public const string STORE_UNLOCKED = "STORE_UNLOCKED ";

        //RVAd
        public const string AD_DELETED = "AD_DELETED";
        public const string AD_EDITED = "AD_EDITED";
        public const string AD_INSPECTED = "AD_INSPECTED";

        //SELL_ITEM
        public const string SELL_ITEM_APPROVED = "SELL_ITEM_APPROVED";
        public const string SELL_ITEM_DECLINED = "SELL_ITEM_DECLINED";
        
        public const string PAYMENT_RECEIVED = "PAYMENT_RECEIVED";

        // Order Product 
        public const string SITE_ORDER2ADMIN_EMAIL = "SITE_ORDER2ADMIN_EMAIL";
        public const string SITE_ORDER2CUSTOMER_EMAIL = "SITE_ORDER2CUSTOMER_EMAIL";
        public const string SITE_ORDER_BUYER = "SITE_ORDER_BUYER";
        public const string SITE_ORDER_INFO = "SITE_ORDER_INFO";
        public const string SITE_ORDER_PAYMENTS = "SITE_ORDER_PAYMENTS";
        
        //Affiliate
        public const string AFFILIATE_REGISTER_CODE = "AFFILIATE_REGISTER_CODE";
        public const string AFFILIATE_INTRO_CODE = "AFFILIATE_INTRO_CODE";
        public const string AFFILIATE_SEND_WISH = "AFFILIATE_SEND_WISH";
        public const string AFFILIATE_SHARE_WISH = "AFFILIATE_SHARE_WISH";

        //Service
        public const string REGISTER_SERVICE = "REGISTER_SERVICE";

        //Reputation system
        public const string REP_SYS_STORE_COMMENT_OWNER_ANNOUNCEMENT = "REP_SYS_STORE_COMMENT_OWNER_ANNOUNCEMENT ";
        public const string REP_SYS_STORE_COMMENT_ACCOUNT_ANNOUNCEMENT = "REP_SYS_STORE_COMMENT_ACCOUNT_ANNOUNCEMENT";
        public const string REP_SYS_PRODUCT_COMMENT_ACCOUNT_ANNOUNCEMENT = "REP_SYS_PRODUCT_COMMENT_ACCOUNT_ANNOUNCEMENT";
    }
}
