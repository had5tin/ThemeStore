using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Falcon.Data.Domain
{
    public class Permission_GetByRoleIdResult
    {
        public string ResourceName { get; set; }

        public string Privilege { get; set; }

        public int RoleId { get; set; }

        public int ResourceId { get; set; }

        public bool IsAllowed { get; set; }
    }
}
