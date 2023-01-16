using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Model;

namespace BusinessLayer.Interface
{
    public interface IRepository<T> where T : BaseModel
    {
        IEnumerable<T> GetAll();
        T GetById(long Id);
        Task<long> Insert(T entity);
        Task <long> Update(T entity);
        void Delete(long Id);
    }
}
