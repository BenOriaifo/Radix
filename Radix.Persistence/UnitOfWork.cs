using Radix.Core;
using System;
using System.Collections.Generic;
using System.Text;
using Radix.Core.Repositories;
using Radix.Data;
using Radix.Persistence.Repositories;

namespace Radix.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly RadixNotificationContext _context;

        public UnitOfWork(RadixNotificationContext context)
        {
            this._context = context;

            AdhocMessages = new AdhocMessageRepository(_context);
            Messages = new MessageRepository(_context);
            MessageTypes = new MessageTypeRepository(_context);
            MessageTemplates = new MessageTemplateRepository(_context);
            NotificationPreferrences = new NotificationPreferrenceRepository(_context);
            NotificationSubscriptions = new NotificationSubscriptionRepository(_context);
            ServiceConfigurations = new ServiceConfigurationRepository(_context);

        }
        public IAdhocMessageRepository AdhocMessages { get; private set; }
        public IMessageRepository Messages { get; private set; }
        public IMessageTypeRepository MessageTypes { get; private set; }
        public IMessageTemplateRepository MessageTemplates { get; private set; }
        public INotificationPreferrenceRepository NotificationPreferrences { get; private set; }
        public INotificationSubscriptionRepository NotificationSubscriptions { get; private set; }
        public IServiceConfigurationRepository ServiceConfigurations { get; private set; }

        public int Complete()
        {
            try
            {
                return _context.SaveChanges();
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return 0;
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
