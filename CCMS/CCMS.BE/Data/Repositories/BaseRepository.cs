using CCMS.BE.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CCMS.BE.Data.Repositories;

public class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected ApplicationDbContext _context;
    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<T> GetByIdAsync(Guid id)
    {
        var item = await _context.Set<T>().FindAsync(id);
        return item;
    }
    public async Task<IEnumerable<T>> GetAllAsync()
    {
        var items = await _context.Set<T>().ToListAsync();
        return items;
    }
    public async Task<T> AddAsync(T entity)
    {
        await _context.Set<T>().AddAsync(entity);
        return entity;
    }
    public T Update(T entity)
    {
        _context.Update(entity);
        return entity;
    }
    public void Delete(T entity)
    {
        _context.Set<T>().Remove(entity);
    }
}

