using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Radix.Core.Models;
using Radix.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Service.Abstract
{
    public interface IMessageService : IService<Message>
    {
        DataTablesResponse SearchApi(IDataTablesRequest requestModel, AdvancedSearchViewModel searchViewModel);
        bool UpdateMessageRequestStatus(Message message);
        bool SendMessage(Message message);
        bool SendMessage(Message message, ref string mgs);
    }
}
