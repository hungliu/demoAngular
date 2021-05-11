using Demo3.DAL.Interface;
using Demo3.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Demo3.DAL.Repositories
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        protected DemoDbContext _db { get; set; }
        protected DbSet<T> _table = null;
        public Repository(DemoDbContext db)
        {
            _db = db;
            _table = _db.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _table.ToList();
        }

        public T GetInfoByID(object id)
        {
            return _table.Find(id);
        }

        public async Task Insert(T obj)
        {
             await _table.AddAsync(obj);
             await _db.SaveChangesAsync();
        }

        public async Task Update(T obj)
        {
            _db.Entry(obj).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public async Task Delete(T obj)
        {
            _table.Remove(obj);
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }

        public T Get(Expression<Func<T, bool>> where)
        {
            return _table.Where(where).FirstOrDefault<T>();
        }
    }
}
