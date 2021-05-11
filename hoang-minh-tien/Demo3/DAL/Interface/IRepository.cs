using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo3.DAL.Interface
{
    public interface IRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetInfoByID(Object id);
        T Get(Expression<Func<T, bool>> where);
        Task Insert(T obj);
        Task Update(T obj);
        Task Delete(T obj);
    }
}
