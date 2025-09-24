using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FitFlex.Domain.Entities.Users_Model;
using FitFlex.Infrastructure.Db_context;
using FitFlex.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FitFlex.Infrastructure.Repository_service
{
    public class repository<T> : IRepository<T> where T : class
    {
        private readonly MyContext _myContext;
        private readonly DbSet<T> _dbset;
        public repository(MyContext context)
        {
            _myContext = context;
            _dbset = context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _dbset.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbset.FindAsync(id);
            
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _myContext.SaveChangesAsync();
        }

        public void Update(T entity)
        {
            _dbset.Update(entity);
        }
        public IQueryable<T> GetAllQueryable()
        {
            return _dbset.AsQueryable();
        }

    }
}
