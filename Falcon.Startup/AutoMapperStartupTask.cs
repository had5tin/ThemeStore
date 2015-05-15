using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Falcon.Tasks;
using AutoMapper;
using Falcon.Data.Domain;
using Falcon.Admin.CoreModules.Models;
using Falcon.Admin.Modules.Contents.Models;

namespace Falcon.Startup
{
    public class AutoMapperStartupTask : IStartupTask
    {

        public void Execute()
        {
            ViceVersa<User, CreateUserModel>();
            ViceVersa<User, EditUserModel>();            
        }

        protected virtual void ViceVersa<T1, T2>()
        {
            Mapper.CreateMap<T1, T2>();
            Mapper.CreateMap<T2, T1>();
        }

        public int Order
        {
            get { return 0; }
        }
    }
}
