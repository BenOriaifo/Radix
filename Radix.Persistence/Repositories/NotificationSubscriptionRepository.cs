using Microsoft.EntityFrameworkCore;
using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Persistence.Repositories
{
    public class NotificationSubscriptionRepository : Repository<NotificationSubscription>, INotificationSubscriptionRepository
    {
        public NotificationSubscriptionRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(NotificationSubscription obj)
        {
            NotificationSubscription objLocal = null;
            try
            {
                objLocal = RadixContext.NotificationSubscriptions.First<NotificationSubscription>(m => m.Pin == obj.Pin);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return objLocal != null ? true : false;
        }

        public override IEnumerable<NotificationSubscription> GetAll()
        {
            return RadixContext.NotificationSubscriptions.Include(n => n.NotificationPreferrences);
        }
    }
}
