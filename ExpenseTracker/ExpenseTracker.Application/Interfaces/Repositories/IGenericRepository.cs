using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);  // New method
        Task<T> FindOneAsync(Expression<Func<T, bool>> predicate);            // New method
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        void UpdateAsync(T entity);
        Task SaveAsync();
    }


}
