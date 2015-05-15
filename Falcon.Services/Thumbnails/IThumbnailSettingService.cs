using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;
using Falcon.Common;

namespace Falcon.Services.Thumbnails
{
    public interface IThumbnailSettingService : IService
	{
        ThumbnailSetting GetByThumbSize(ThumbSizeEnum thumbSize);
		int Add(ThumbnailSetting entry);
		void Update(ThumbnailSetting entry);
		void Remove(ThumbnailSetting entry);
		IEnumerable<ThumbnailSetting> GetAll();
	}
}