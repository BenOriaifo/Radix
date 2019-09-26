using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Linq;

namespace Radix.Persistence.Repositories
{
    public class MessageTemplateRepository : Repository<MessageTemplate>, IMessageTemplateRepository
    {
        public MessageTemplateRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(MessageTemplate obj)
        {
            MessageTemplate objLocal = null;
            try
            {
                objLocal = RadixContext.MessageTemplates.First<MessageTemplate>(m => m.MessageType.Code == obj.MessageType.Code);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return objLocal != null ? true : false;
        }
    }
}
