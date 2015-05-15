using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Falcon.Data
{
    public interface IDatabaseFactory
    {
        Database GetDatabase();
    }
}
