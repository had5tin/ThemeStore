using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Falcon.Data.Repository;
using Falcon.Data.Domain;
using Falcon.Common;

namespace Falcon.Services.Thumbnails
{
    public class ThumbnailSettingService : BaseService, IThumbnailSettingService
	{
		private readonly IThumbnailSettingRepository _thumbnailsettingRepository;

		public ThumbnailSettingService(IThumbnailSettingRepository thumbnailsettingRepository)
		{
			this._thumbnailsettingRepository = thumbnailsettingRepository;
		}

        public ThumbnailSetting GetByThumbSize(ThumbSizeEnum thumbSize)
		{
			return _thumbnailsettingRepository.Table.SingleOrDefault(c => c.ThumbSize == thumbSize.ToString());
		}

		public int Add(ThumbnailSetting thumbnailsetting)
		{
			 _thumbnailsettingRepository.Add(thumbnailsetting);
			return 1;
		}

		public void Update(ThumbnailSetting thumbnailsetting)
		{
			 _thumbnailsettingRepository.SubmitChanges();
		}

		public void Remove(ThumbnailSetting thumbnailsetting)
		{
			 _thumbnailsettingRepository.Remove(thumbnailsetting);
		}

		public IEnumerable<ThumbnailSetting> GetAll()
		{
			return _thumbnailsettingRepository.Table.ToList();
		}

	}
}