using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp.Repository
{
    public interface IRepository <T> where T : BaseEntity
    {
        public T Add(T entity);

        public void Update(T entity);
        public void Delete(int id);
        public T GetById(int id);
        public IEnumerable<T> GetAll();

    }
}
