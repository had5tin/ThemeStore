using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Falcon.Data.Domain;
using AutoMapper;

namespace Falcon.Admin.CoreModules.Models
{
    public static class MappingExtensions
    {
        #region CreateUserModel
        public static CreateUserModel ToCreateModel(this User entity)
        {
            return Mapper.Map<User, CreateUserModel>(entity);
        }

        public static User ToEntity(this CreateUserModel model)
        {
            return Mapper.Map<CreateUserModel, User>(model);
        }
        #endregion

        #region EditUserModel
        public static EditUserModel ToEditModel(this User entity)
        {
            return Mapper.Map<User, EditUserModel>(entity);
        }

        public static User ToEntity(this EditUserModel model)
        {
            return Mapper.Map<EditUserModel, User>(model);
        }
        #endregion

        #region RoleModel
        public static RoleModel ToModel(this Role entity)
        {
            return Mapper.Map<Role, RoleModel>(entity);
        }

        public static Role ToEntity(this RoleModel model)
        {
            return Mapper.Map<RoleModel, Role>(model);
        }
        #endregion

        #region SystemSettingModel
        public static SystemSettingModel ToCreateModel(this SystemSetting entity)
        {
            return Mapper.Map<SystemSetting, SystemSettingModel>(entity);
        }

        public static SystemSetting ToEntity(this SystemSettingModel model)
        {
            return Mapper.Map<SystemSettingModel, SystemSetting>(model);
        }
        #endregion

        #region ThemeModel
        public static ThemeModel ToModel(this Theme entity)
        {
            return Mapper.Map<Theme, ThemeModel>(entity);
        }

        public static Theme ToEntity(this ThemeModel model)
        {
            return Mapper.Map<ThemeModel, Theme>(model);
        }
        #endregion  

    }
}