using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Falcon.Data.Domain;
using Falcon.Data.Map;

namespace Falcon.Data
{
    public partial class Database : DbContext, IDatabase
    {
        //public IDbSet<Asset> Assets { get; set; } // Assets
        //public IDbSet<City> Cities { get; set; } // Cities
        //public IDbSet<District> Districts { get; set; } // Districts

        public IDbSet<Feature> Features { get; set; } // Features
        public IDbSet<Collection> Collections { get; set; } // Collections
        public IDbSet<ThemeStore> ThemeStores { get; set; } // Collections
        public IDbSet<ThemeFeature> ThemeFeatures { get; set; } // ThemeFeatures
        public IDbSet<ThemeColletion> ThemeColletions { get; set; } // ThemeColletions
        public IDbSet<Permission> Permissions { get; set; } // Permissions
        public IDbSet<Resource> Resources { get; set; } // Resources
        public IDbSet<Role> Roles { get; set; } // Roles
        public IDbSet<SystemSetting> SystemSettings { get; set; } // SystemSettings
        public IDbSet<Theme> Themes { get; set; } // Themes
        public IDbSet<ThumbnailSetting> ThumbnailSettings { get; set; } // ThumbnailSettings
        public IDbSet<User> Users { get; set; } // Users
        public IDbSet<UserRole> UserRoles { get; set; } // UserRoles
        public IDbSet<StaticPage> StaticPages { get; set; } // StaticPages
        //public IDbSet<ReceivedMessage> ReceivedMessages { get; set; } // Received_Messages
        //public IDbSet<SentMessage> SentMessages { get; set; } // Sent_Messages
        //public IDbSet<HtmlBlock> HtmlBlocks { get; set; } // HtmlBlocks
        //public IDbSet<SeoUrl> SeoUrls { get; set; } // Seo_Urls
        //public IDbSet<SeoUrlGroup> SeoUrlGroups { get; set; } // Seo_UrlGroup
        public Database()
            : base("Name=HangTotReviews_NewContext")
        {
            InitializePartial();
        }

        public Database(string connectionString)
            : base(connectionString)
        {
            InitializePartial();
        }

        public Database(string connectionString, System.Data.Entity.Infrastructure.DbCompiledModel model)
            : base(connectionString, model)
        {
            InitializePartial();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //modelBuilder.Configurations.Add(new AssetMap());
            //modelBuilder.Configurations.Add(new CityMap());
            //modelBuilder.Configurations.Add(new DistrictMap());
            modelBuilder.Configurations.Add(new PermissionMap());
            modelBuilder.Configurations.Add(new ResourceMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new SystemSettingMap());
            modelBuilder.Configurations.Add(new ThemeMap());
            modelBuilder.Configurations.Add(new ThumbnailSettingMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new StaticPageMap());
            modelBuilder.Configurations.Add(new FeatureMap());
            modelBuilder.Configurations.Add(new CollectionMap());
            modelBuilder.Configurations.Add(new ThemeStoreMap());
            modelBuilder.Configurations.Add(new ThemeFeatureMap());
            modelBuilder.Configurations.Add(new ThemeColletionMap());

            //modelBuilder.Configurations.Add(new ReceivedMessageMap());
            //modelBuilder.Configurations.Add(new SentMessageMap());
            //modelBuilder.Configurations.Add(new HtmlBlockMap());
            //modelBuilder.Configurations.Add(new SeoUrlMap());
            //modelBuilder.Configurations.Add(new SeoUrlGroupMap());
            OnModelCreatingPartial(modelBuilder);
        }

        public static DbModelBuilder CreateModel(DbModelBuilder modelBuilder, string schema)
        {
            //modelBuilder.Configurations.Add(new AssetMap(schema));
            //modelBuilder.Configurations.Add(new CityMap(schema));
            //modelBuilder.Configurations.Add(new DistrictMap(schema));
            modelBuilder.Configurations.Add(new ResourceMap(schema));
            modelBuilder.Configurations.Add(new RoleMap(schema));
            modelBuilder.Configurations.Add(new SystemSettingMap(schema));
            modelBuilder.Configurations.Add(new ThemeMap(schema));
            modelBuilder.Configurations.Add(new ThumbnailSettingMap(schema));
            modelBuilder.Configurations.Add(new UserMap(schema));
            modelBuilder.Configurations.Add(new UserRoleMap(schema));
            modelBuilder.Configurations.Add(new StaticPageMap(schema));
            modelBuilder.Configurations.Add(new FeatureMap(schema));
            modelBuilder.Configurations.Add(new CollectionMap(schema));
            modelBuilder.Configurations.Add(new ThemeStoreMap(schema));
            modelBuilder.Configurations.Add(new ThemeFeatureMap(schema));
            modelBuilder.Configurations.Add(new ThemeColletionMap(schema));

            //modelBuilder.Configurations.Add(new ReceivedMessageMap(schema));
            //modelBuilder.Configurations.Add(new SentMessageMap(schema));
            //modelBuilder.Configurations.Add(new HtmlBlockMap(schema));
            //modelBuilder.Configurations.Add(new SeoUrlMap(schema));
            //modelBuilder.Configurations.Add(new SeoUrlGroupMap(schema));

            return modelBuilder;
        }

        partial void InitializePartial();
        partial void OnModelCreatingPartial(DbModelBuilder modelBuilder);
    }
}
