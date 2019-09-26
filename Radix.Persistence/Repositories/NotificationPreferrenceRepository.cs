using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Linq;

namespace Radix.Persistence.Repositories
{
    public class NotificationPreferrenceRepository : Repository<NotificationPreferrence>, INotificationPreferrenceRepository
    {
        public NotificationPreferrenceRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(NotificationPreferrence obj)
        {
            throw new NotImplementedException();
        }
    }
}
