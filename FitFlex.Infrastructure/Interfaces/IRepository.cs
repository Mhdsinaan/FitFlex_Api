using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Entities.Users_Model;

namespace FitFlex.Infrastructure.Interfaces
{
    public interface IRepository<T> where T:class
    {
        Task<T> GetByIdAsync(int id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
         void Update(T entity);
        void Delete(T entity);
        Task<int> SaveChangesAsync();
        IQueryable<T> GetAllQueryable();
    }
}
