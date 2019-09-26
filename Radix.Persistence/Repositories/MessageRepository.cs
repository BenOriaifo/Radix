using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Radix.Persistence.Repositories
{
    public class MessageRepository : Repository<Message>, IMessageRepository
    {
        public MessageRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(Message obj)
        {
            throw new NotImplementedException();
        }
    }
}
