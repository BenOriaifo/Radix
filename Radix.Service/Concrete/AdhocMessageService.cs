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
    public class AdhocMessageService : IAdhocMessageService
    {
        IUnitOfWork uow;

        public AdhocMessageService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public bool Save(AdhocMessage obj, ref string message)
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

        private bool Add(AdhocMessage obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            if (!uow.AdhocMessages.IsExists(obj))
            {
                uow.AdhocMessages.Add(obj);
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

        private bool Update(long Id, AdhocMessage obj)
        {
            bool state = false;

            var objEx = uow.AdhocMessages.Get(obj.Id);
            objEx = obj;
            objEx.Id = Id;
            uow.AdhocMessages.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(AdhocMessage obj)
        {
            bool state = false;

            uow.AdhocMessages.Remove(obj);
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

            var obj = uow.AdhocMessages.Get(id);

            uow.AdhocMessages.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public AdhocMessage GetById(long Id)
        {
            return uow.AdhocMessages.Get(Id);
        }

        public IEnumerable<AdhocMessage> GetAll()
        {
            return uow.AdhocMessages.GetAll().ToList();
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            IQueryable<AdhocMessage> query = uow.AdhocMessages.GetAll().AsQueryable();

            var totalCount = query.Count();

            // Apply filters
            if (!string.IsNullOrEmpty(requestModel.Search.Value))
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Title.Contains(value)
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
                                      Title = tr.Title,
                                      Status = tr.Status,
                                      DateCreated = tr.DateCreated
                                  };

            DataTablesResponse response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            return response;
        }
    }
}
