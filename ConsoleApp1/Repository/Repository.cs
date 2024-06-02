using ConsoleApp.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Repository
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly FLDBContext context;
        private readonly DbSet<T> dbSet;
        public Repository()
        {
            context = new FLDBContext();
            dbSet = context.Set<T>();

        }
        public T Add(T entity)
        {
            dbSet.Add(entity);    
            return entity;

        }

        public void Delete(int id)
        {
            T entity = GetById(id);
            if (entity != null)
            {
                dbSet.Remove(entity);
                context.SaveChanges();
            }
        }

        public IEnumerable<T> GetAll()
        {
            return dbSet.ToList();

        }

        public T GetById(int id)
        {
            return dbSet.FirstOrDefault(x => x.Id == id);
        }

        public void Update(T entity)
        {


            dbSet.Update(entity);
            context.SaveChanges();

        }
    }
}
