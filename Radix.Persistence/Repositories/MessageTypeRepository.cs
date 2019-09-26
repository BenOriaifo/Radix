using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Linq;

namespace Radix.Persistence.Repositories
{
    public class MessageTypeRepository : Repository<MessageType>, IMessageTypeRepository
    {
        public MessageTypeRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(MessageType obj)
        {
            MessageType objLocal = null;
            try
            {
                objLocal = RadixContext.MessageTypes.First<MessageType>(m => m.Code == obj.Code);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return objLocal != null ? true : false;
        }
    }
}
