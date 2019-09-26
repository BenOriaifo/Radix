using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Radix.Core;
using Radix.Core.Models;
using Radix.Service.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Radix.Service.Concrete
{
    public class ServiceConfigurationService : IServiceConfigurationService
    {
        IUnitOfWork uow;

        public ServiceConfigurationService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public bool Save(ServiceConfiguration obj, ref string message)
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

        private bool Add(ServiceConfiguration obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            if (!uow.ServiceConfigurations.IsExists(obj))
            {
                uow.ServiceConfigurations.Add(obj);
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

        private bool Update(long Id, ServiceConfiguration obj)
        {
            bool state = false;

            var objEx = uow.ServiceConfigurations.Get(obj.Id);
            objEx = obj;
            objEx.Id = (int)Id;
            uow.ServiceConfigurations.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(ServiceConfiguration obj)
        {
            bool state = false;

            uow.ServiceConfigurations.Remove(obj);
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

            var obj = uow.ServiceConfigurations.Get(id);

            uow.ServiceConfigurations.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public ServiceConfiguration GetById(long Id)
        {
            return uow.ServiceConfigurations.Get(Id);
        }

        public IEnumerable<ServiceConfiguration> GetAll()
        {
            return uow.ServiceConfigurations.GetAll().ToList();
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            IQueryable<ServiceConfiguration> query = uow.ServiceConfigurations.GetAll().AsQueryable();

            var totalCount = query.Count();

            // Apply filters
            if (!string.IsNullOrEmpty(requestModel.Search.Value))
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.MessageTypeId.ToString().Contains(value)||
                                         p.MaximumRecordsToFetch.ToString().Contains(value)||
                                         p.WaitTime.ToString().Contains(value)
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
                                      MaxRecordsToFetch = tr.MaximumRecordsToFetch,
                                      MessageType = tr.MessageType.Code,
                                      WaitTime = tr.WaitTime
                                  };

            DataTablesResponse response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            return response;
        }
    }
}
