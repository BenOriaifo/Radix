using Radix.Core.Models;
using Radix.Core.Repositories;
using Radix.Data;
using System;
using System.Linq;

namespace Radix.Persistence.Repositories
{
    public class AdhocMessageRepository : Repository<AdhocMessage>, IAdhocMessageRepository
    {
        public AdhocMessageRepository(RadixNotificationContext context)
            : base(context)
        { }

        public RadixNotificationContext RadixContext
        {
            get { return Context as RadixNotificationContext; }
        }

        public bool IsExists(AdhocMessage obj)
        {
            AdhocMessage objLocal = null;
            try
            {
                objLocal = RadixContext.AdhocMessages.First<AdhocMessage>(m => m.Title == obj.Title);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return objLocal != null ? true : false;
        }
    }
}
