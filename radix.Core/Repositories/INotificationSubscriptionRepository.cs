using Radix.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Core.Repositories
{
    public interface INotificationSubscriptionRepository : IRepository<NotificationSubscription>
    {
        bool IsExists(NotificationSubscription obj);
    }
}
