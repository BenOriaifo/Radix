using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Radix.Core;
using Radix.Core.Models;
using Radix.Service.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Service.Concrete
{
    public class NotificationPreferrenceService : INotificationPreferrenceService
    {
        IUnitOfWork uow;

        public NotificationPreferrenceService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public bool Save(NotificationPreferrence obj, ref string message)
        {
            if (obj.Id == 0)
            {
                return Add(obj, ref message);
            }
            else
            {
                return Update(obj.Id, obj);
            }
        }

        private bool Add(NotificationPreferrence obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            if (!uow.NotificationPreferrences.IsExists(obj))
            {
                uow.NotificationPreferrences.Add(obj);
                int result = uow.Complete();
                if (result > 0)
                {
                    state = true;
                }
            }
            else
            {
                message = "Data Exists!";
            }

            return state;
        }

        private bool Update(long Id, NotificationPreferrence obj)
        {
            bool state = false;

            var objEx = uow.NotificationPreferrences.Get(obj.Id);
            objEx = obj;
            objEx.Id = (int)Id;
            uow.NotificationPreferrences.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(NotificationPreferrence obj)
        {
            bool state = false;

            uow.NotificationPreferrences.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(long id)
        {
            bool state = false;

            var obj = uow.NotificationPreferrences.Get(id);

            uow.NotificationPreferrences.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public NotificationPreferrence GetById(long Id)
        {
            return uow.NotificationPreferrences.Get(Id);
        }

        public IEnumerable<NotificationPreferrence> GetAll()
        {
            return uow.NotificationPreferrences.GetAll().ToList();
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            IQueryable<NotificationPreferrence> query = uow.NotificationPreferrences.GetAll().AsQueryable();

            var totalCount = query.Count();

            // Apply filters
            if (requestModel.Search.Value != string.Empty)
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.MessageType.Code.Contains(value) 
                                    );
            }

            var filteredCount = query.Count();

            // Sorting
            var orderColums = requestModel.Columns.Where(x => x.Sort != null);

            //paging
            var data = query.OrderBy(orderColums).Skip(requestModel.Start).Take(requestModel.Length);

            var transformedData = from tr in data
                                  select new
                                  {
                                      Id = tr.Id,
                                      MessageType = tr.MessageType.Code,
                                      FullName = tr.NotificationSubscription.FullName,
                                      Pin = tr.NotificationSubscription.Pin,
                                      Mobile = tr.NotificationSubscription.Mobile,
                                      Email = tr.NotificationSubscription.Email
                                  };

            DataTablesResponse response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            return response;
        }
    }
}
