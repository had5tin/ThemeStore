using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Falcon.Data.Domain
{
    public partial class Permission
    {
        [NotMapped]
        public string RoleName { get; set; }
        [NotMapped]
        public string ResourceName { get; set; }
    }
}
