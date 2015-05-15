using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
	public class ThumbnailSettingRepository : BaseRepository<ThumbnailSetting>, IThumbnailSettingRepository
	{
		public ThumbnailSettingRepository(Database database)
			: base(database)
		{
		}

		public ThumbnailSettingRepository(IDatabaseFactory factory)
			: base(factory)
		{
		}

	}
}