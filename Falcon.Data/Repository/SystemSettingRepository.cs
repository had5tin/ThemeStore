using System;
using System.Collections.Generic;
using System.Text;
using Falcon.Data.Domain;

namespace Falcon.Data.Repository
{
	public class SystemSettingRepository : BaseRepository<SystemSetting>, ISystemSettingRepository
	{
		public SystemSettingRepository(Database database)
			: base(database)
		{
		}

		public SystemSettingRepository(IDatabaseFactory factory)
			: base(factory)
		{
		}

	}
}