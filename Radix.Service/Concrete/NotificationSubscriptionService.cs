using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Radix.Core;
using Radix.Core.Models;
using Radix.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Service.Concrete
{
    public class NotificationSubscriptionService : INotificationSubscriptionService
    {
        IUnitOfWork uow;

        public NotificationSubscriptionService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public bool Save(NotificationSubscription obj, ref string message)
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

        private bool Add(NotificationSubscription obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            if (!uow.NotificationSubscriptions.IsExists(obj))
            {
                uow.NotificationSubscriptions.Add(obj);
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

        private bool Update(long Id, NotificationSubscription obj)
        {
            bool state = false;

            var objEx = uow.NotificationSubscriptions.Get(obj.Id);
            objEx = obj;
            objEx.Id = (int)Id;
            uow.NotificationSubscriptions.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(NotificationSubscription obj)
        {
            bool state = false;

            uow.NotificationSubscriptions.Remove(obj);
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

            var obj = uow.NotificationSubscriptions.Get(id);

            uow.NotificationSubscriptions.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public NotificationSubscription GetById(long Id)
        {
            return uow.NotificationSubscriptions.Get(Id);
        }

        public IEnumerable<NotificationSubscription> GetAll()
        {
            return uow.NotificationSubscriptions.GetAll().ToList();
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            IQueryable<NotificationSubscription> query = uow.NotificationSubscriptions.GetAll().AsQueryable();

            var totalCount = query.Count();

            // Apply filters
            if (!string.IsNullOrEmpty(requestModel.Search.Value))
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.FullName.ToLower().Contains(value.ToLower()) ||
                                         p.Mobile.ToLower().Contains(value.ToLower()) ||
                                         p.Pin.ToLower().Contains(value.ToLower())
                                         //p.Email.Contains(value)
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
                                      FullName = tr.FullName,
                                      Pin = tr.Pin,
                                      Mobile = tr.Mobile,
                                      Email = tr.Email
                                  };

            DataTablesResponse response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            return response;
        }
    }
}
