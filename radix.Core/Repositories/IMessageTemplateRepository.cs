using Radix.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Core.Repositories
{
    public interface IMessageTemplateRepository : IRepository<MessageTemplate>
    {
        bool IsExists(MessageTemplate obj);
    }
}
