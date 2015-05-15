using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data.Domain;
using Falcon.Data.Map;

namespace Falcon.Data
{
    public interface IDatabase : IDisposable
    {
        //IDbSet<Asset> Assets { get; set; } // Assets
        //IDbSet<City> Cities { get; set; } // Cities
        //IDbSet<District> Districts { get; set; } // Districts

        IDbSet<Feature> Features { get; set; } // Features
        IDbSet<Permission> Permissions { get; set; } // Permissions
        IDbSet<Resource> Resources { get; set; } // Resources
        IDbSet<Role> Roles { get; set; } // Roles
        IDbSet<SystemSetting> SystemSettings { get; set; } // SystemSettings
        IDbSet<Theme> Themes { get; set; } // Themes
        IDbSet<ThumbnailSetting> ThumbnailSettings { get; set; } // ThumbnailSettings
        IDbSet<User> Users { get; set; } // Users
        IDbSet<UserRole> UserRoles { get; set; } // Users
        IDbSet<StaticPage> StaticPages { get; set; } // StaticPages
        //<ReceivedMessage> ReceivedMessages { get; set; } // Received_Messages
        //IDbSet<SentMessage> SentMessages { get; set; } // Sent_Messages
        //IDbSet<HtmlBlock> HtmlBlocks { get; set; } // HtmlBlocks
        //IDbSet<SeoUrl> SeoUrls { get; set; } // Seo_Urls
        //IDbSet<SeoUrlGroup> SeoUrlGroups { get; set; } // Seo_UrlGroup
        int SaveChanges();
    }

}
