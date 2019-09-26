using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Radix.Persistence.Repositories
{
    public class ServiceConfigurationRepository : Repository<ServiceConfiguration>, IServiceConfigurationRepository
    {
        public ServiceConfigurationRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(ServiceConfiguration obj)
        {
            ServiceConfiguration actor = null;
            try
            {
                actor = RadixContext.ServiceConfigurations.First<ServiceConfiguration>(m => m.MessageType.Code == obj.MessageType.Code);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return actor != null ? true : false;
        }

        public override IEnumerable<ServiceConfiguration> GetAll()
        {
            return RadixContext.ServiceConfigurations.Include(s => s.MessageType);
        }
    }
}
