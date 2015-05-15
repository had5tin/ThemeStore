using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Falcon.Data
{
    public abstract partial class BaseEntity
    {
        public virtual int Id { get; set; }
    }
}
