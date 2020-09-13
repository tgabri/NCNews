using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCNews.API.Contracts
{
    public interface IRepositoryBase<T> where T : class
    {
        Task<IList<T>> FindAll();
        Task<T> FindById(int id);
        Task<bool> Create(T model);
        Task<bool> IsExists(int id);
        Task<bool> Update(T model);
        Task<bool> Delete(T model);
        Task<bool> Save();
    }
}
