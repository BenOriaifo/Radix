using System;
using System.Collections.Generic;
using System.Text;
using Radix.Core.Repositories;

namespace Radix.Core
{
    public interface IUnitOfWork : IDisposable
    {
        IAdhocMessageRepository AdhocMessages { get; }
        IMessageRepository Messages { get; }
        IMessageTypeRepository MessageTypes { get; }
        IMessageTemplateRepository MessageTemplates { get; }
        INotificationPreferrenceRepository NotificationPreferrences { get; }
        INotificationSubscriptionRepository NotificationSubscriptions { get; }
        IServiceConfigurationRepository ServiceConfigurations { get; }

        int Complete();
    }
}