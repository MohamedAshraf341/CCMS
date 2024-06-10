using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Interfaces;

public interface IBaseRepository<T> where T : class
{
    Task<T> GetByIdAsync(Guid id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<T> AddAsync(T entity);
    T Update(T entity);
    void Delete(T entity);
}
