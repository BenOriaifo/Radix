using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using Radix.Core;
using Radix.Core.Models;
using Radix.Service.Abstract;
using System.Collections.Generic;
using System.Linq;

namespace Radix.Service.Concrete
{
    public class MessageTemplateService : IMessageTemplateService
    {
        IUnitOfWork uow;

        public MessageTemplateService(IUnitOfWork uow)
        {
            this.uow = uow;
        }

        public bool Save(MessageTemplate obj, ref string message)
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

        private bool Add(MessageTemplate obj, ref string message)
        {
            bool state = false;

            // Check if there is an existing name
            if (!uow.MessageTemplates.IsExists(obj))
            {
                uow.MessageTemplates.Add(obj);
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

        private bool Update(long Id, MessageTemplate obj)
        {
            bool state = false;

            var objEx = uow.MessageTemplates.Get(obj.Id);
            objEx = obj;
            objEx.Id = (int)Id;
            uow.MessageTemplates.Update(Id, objEx);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public bool Remove(MessageTemplate obj)
        {
            bool state = false;

            uow.MessageTemplates.Remove(obj);
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

            var obj = uow.MessageTemplates.Get(id);

            uow.MessageTemplates.Remove(obj);
            int result = uow.Complete();
            if (result > 0)
            {
                state = true;
            }
            return state;
        }

        public MessageTemplate GetById(long Id)
        {
            return uow.MessageTemplates.Get(Id);
        }

        public IEnumerable<MessageTemplate> GetAll()
        {
            return uow.MessageTemplates.GetAll().ToList();
        }

        public DataTablesResponse SearchApi(IDataTablesRequest requestModel)
        {
            IQueryable<MessageTemplate> query = uow.MessageTemplates.GetAll().AsQueryable();

            var totalCount = query.Count();

            // Apply filters
            if (!string.IsNullOrEmpty(requestModel.Search.Value))
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
                                      Code = tr.MessageType.Code                                    
                                  };

            DataTablesResponse response = DataTablesResponse.Create(requestModel, totalCount, filteredCount, transformedData);
            return response;
        }
    }
}
