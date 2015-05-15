/// <copyright file="SystemSettingService.cs" company="DKT">
/// Copyright (c) 2012 DKT Technology.  All rights reserved.
/// </copyright>
/// <author>Khôi Nguyễn Minh</author>
/// <email>khoinm@dkt.com.vn</email>
/// <description></description>

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Falcon.Data.Repository;
using Falcon.Data.Domain;
using Falcon.Caching;
using Falcon.Infrastructure;
using System.ComponentModel;
using Falcon.Caching;

namespace Falcon.Services.SystemSettings
{
	public class SystemSettingService : BaseService, ISystemSettingService
	{
        private readonly ISystemSettingRepository _systemsettingRepository;

        public SystemSettingService(ISystemSettingRepository systemsettingRepository)
        {
            this._systemsettingRepository = systemsettingRepository;
        }

        public SystemSetting GetForUpdate(string key)
        {
            return _systemsettingRepository.Table.SingleOrDefault(c => c.SettingKey == key);
        }

        /// <summary>
        /// Lấy thiết lập hệ thống sử dụng để update phía quản trị
        /// </summary>
        /// <param name="key">khóa thiết lập</param>
        /// <returns></returns>
        public SystemSetting GetSystemSetting(string key)
        {
            return AspectF.Define
                .Cache<SystemSetting>(Cache, CoreCacheKeys.SystemSettingKeys.SystemSettingByKey(key))
                .Return<SystemSetting>(() => _systemsettingRepository.Table.SingleOrDefault(c => c.SettingKey == key));    
        }

        /// <summary>
        /// Lấy thiết lập hệ thống, dữ liệu trả về dạng string
        /// </summary>
        /// <param name="key">khóa thiết lập</param>
        /// <returns></returns>
        public string Get(string key)
        {
            SystemSetting setting = GetSystemSetting(key);

            return setting == null ? null : setting.Value;
        }

        /// <summary>
        /// Lấy thiết lập hệ thống, dữ liệu trả về dạng T tùy ý
        /// </summary>
        /// <typeparam name="T">kiểu dữ liệu trả về</typeparam>
        /// <param name="key">khóa thiết lập</param>
        /// <returns></returns>
        public T Get<T>(string key) where T : struct
        {
            SystemSetting setting = GetSystemSetting(key);

            T result = new T();

            try
            {
                if (setting != null || setting.Value.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(setting.Value);
                }
            }
            catch { }

            return result;
        }

        /// <summary>
        /// Lấy thiết lập hệ thống, dữ liệu trả về dạng T tùy ý
        /// </summary>
        /// <typeparam name="T">kiểu dữ liệu trả về</typeparam>
        /// <param name="key">khóa thiết lập</param>
        /// <param name="defaultValue">giá trị mặc định trong trường hợp không tồn tại thiết lập</param>
        /// <returns></returns>
        public T Get<T>(string key, T defaultValue) where T : struct
        {
            SystemSetting setting = GetSystemSetting(key);

            T result = defaultValue;
            try
            {
                if (setting != null || setting.Value.Trim().Length > 0)
                {
                    TypeConverter conv = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)conv.ConvertFrom(setting.Value);
                }
            }
            catch { }

            return result;
        }

        public int AddSystemSetting(SystemSetting systemsetting)
        {
            _systemsettingRepository.Add(systemsetting);
            return 1;
        }

        public void UpdateSystemSetting(SystemSetting systemsetting)
        {
            _systemsettingRepository.SubmitChanges();

            Cache.Remove(CoreCacheKeys.SystemSettingKeys.SystemSettingByKey(systemsetting.SettingKey));
        }

        public void RemoveSystemSetting(SystemSetting systemsetting)
        {
            _systemsettingRepository.Remove(systemsetting);

            Cache.Remove(CoreCacheKeys.SystemSettingKeys.SystemSettingByKey(systemsetting.SettingKey));
        }

        public IEnumerable<SystemSetting> GetAllSystemSetting()
        {
            return _systemsettingRepository.Table.ToList();
        }

        public void UpdateSystemSetting(string key, object value)
        {
            _systemsettingRepository.Execute("update SystemSettings set Value = @Value where setting_key = @SettingKey", new { Value = value.ToString(), SettingKey = key });

            Cache.Remove(CoreCacheKeys.SystemSettingKeys.SystemSettingByKey(key));
        }        
    }
}