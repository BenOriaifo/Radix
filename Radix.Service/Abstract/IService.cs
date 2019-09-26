using DataTables.AspNet.AspNetCore;
using DataTables.AspNet.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Service.Abstract
{
    public interface IService<TEntity> where TEntity: class
    {
        bool Save(TEntity obj, ref string message);
        bool Remove(TEntity obj);
        bool Remove(long id);
        TEntity GetById(long Id);
        IEnumerable<TEntity> GetAll();
        DataTablesResponse SearchApi(IDataTablesRequest requestModel);
    }
}
