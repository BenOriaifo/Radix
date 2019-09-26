using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Radix.Core;
using Radix.Core.Models;
using Radix.Service.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Service.Concrete
{
    public class MessageTypeService : IMessageTypeService
    {
        IUnitOfWork uow;

        public MessageTypeService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public bool Save(MessageType obj, ref string message)
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

        private bool Add(MessageType obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            if (!uow.MessageTypes.IsExists(obj))
            {
                uow.MessageTypes.Add(obj);
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

        private bool Update(long Id, MessageType obj)
        {
            bool state = false;

            var objEx = uow.MessageTypes.Get(obj.Id);
            objEx = obj;
            objEx.Id = (int)Id;
            uow.MessageTypes.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(MessageType obj)
        {
            bool state = false;

            uow.MessageTypes.Remove(obj);
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

            var obj = uow.MessageTypes.Get(id);

            uow.MessageTypes.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public MessageType GetById(long Id)
        {
            return uow.MessageTypes.Get(Id);
        }

        public IEnumerable<MessageType> GetAll()
        {
            return uow.MessageTypes.GetAll().ToList();
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            IQueryable<MessageType> query = uow.MessageTypes.GetAll().AsQueryable();

            var totalCount = query.Count();

            // Apply filters
            if (!string.IsNullOrEmpty(requestModel.Search.Value))
            {
                var value = requestModel.Search.Value.Trim();
                query = query.Where(p => p.Code.Contains(value) ||
                                        p.Type.Contains(value)  
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
                                      Code = tr.Code,
                                      Type = tr.Type
                                      
                                  };

            DataTablesResponse response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            return response;
        }
    }
}
