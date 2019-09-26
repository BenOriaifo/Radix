using Radix.Core.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Core.Repositories
{
    public interface IMessageRepository : IRepository<Message>
    {
        bool IsExists(Message obj);
    }
}
