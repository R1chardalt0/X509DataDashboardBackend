using Microsoft.EntityFrameworkCore;
using Microsoft.FSharp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace X509Data.ChargePadLine.Api.Infrastructure
{
  public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
  {
    private readonly AppDbContext _context;
    public Repository(AppDbContext context)
    {
      _context = context;
    }

    public List<TEntity> GetList()
    {
      var dbSet = _context.Set<TEntity>();
      return dbSet.ToList();
    }

    public List<TEntity> GetList(Func<TEntity, bool> predicate)
    {
      var dbSet = _context.Set<TEntity>();
      return dbSet.Where(predicate).ToList();
    }

    public async Task<List<TEntity>> GetListAsync(string sort, int pageIndex, int pageSize)
    {
      int skip = (pageIndex - 1) * pageSize;
      var dbSet = _context.Set<TEntity>();
      return await dbSet.OrderBy(m => m.GetType().GetProperty(sort).GetValue(m)).Skip(skip).Take(pageSize).ToListAsync();
    }

    public IQueryable<TEntity> GetQueryable()
    {
      var dbSet = _context.Set<TEntity>();
      return dbSet;
    }





    public IQueryable<TEntity> GetQueryableAsync()
    {
      var dbSet = _context.Set<TEntity>();
      return dbSet;
    }



    public async Task<List<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> predicate, string sort, int pageIndex, int pageSize)
    {
      int skip = (pageIndex - 1) * pageSize;
      var dbSet = _context.Set<TEntity>();
      return await dbSet.Where(predicate).OrderBy(m => sort).Skip(skip).Take(pageSize).ToListAsync();
    }

    public TEntity Get(Func<TEntity, bool> predicate)
    {
      var dbSet = _context.Set<TEntity>();
      return dbSet.FirstOrDefault(predicate);
    }
    public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> predicate)
    {
      var dbSet = _context.Set<TEntity>();
      return await dbSet.FirstOrDefaultAsync(predicate);
    }
    public TEntity Insert(TEntity entity)
    {
      var dbSet = _context.Set<TEntity>();
      var res = dbSet.Add(entity).Entity;
      _context.SaveChanges();
      return res;
    }
    public async Task<TEntity> InsertAsync(TEntity entity)
    {
      var dbSet = _context.Set<TEntity>();
      var res = (await dbSet.AddAsync(entity)).Entity;
      await _context.SaveChangesAsync();
      return res;
    }
    public TEntity Delete(TEntity entity)
    {
      var dbSet = _context.Set<TEntity>();
      var res = dbSet.Remove(entity).Entity;
      _context.SaveChanges();
      return res;
    }
    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
      var dbSet = _context.Set<TEntity>();
      var res = dbSet.Remove(entity).Entity;
      await _context.SaveChangesAsync();
      return res;
    }
    public TEntity Update(TEntity entity)
    {
      var dbSet = _context.Set<TEntity>();
      var res = dbSet.Update(entity).Entity;
      _context.SaveChanges();
      return res;
    }
    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
      var dbSet = _context.Set<TEntity>();
      var res = dbSet.Update(entity).Entity;
      await _context.SaveChangesAsync();
      return res;
    }
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
    {
      var dbSet = _context.Set<TEntity>();
      return await dbSet.CountAsync(predicate);
    }
    public async Task<int> CountAsync()
    {
      var dbSet = _context.Set<TEntity>();
      return await dbSet.CountAsync();
    }
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
      var dbSet = _context.Set<TEntity>();
      return await dbSet.AnyAsync(predicate);
    }
    public async Task<bool> AnyAsync()
    {
      var dbSet = _context.Set<TEntity>();
      return await dbSet.AnyAsync();
    }

    public async Task<int> InsertAsyncs(TEntity entity)
    {
      await _context.Set<TEntity>().AddAsync(entity);
      return await _context.SaveChangesAsync();
    }

    public async Task<int> UpdateAsyncs(TEntity entity)
    {
      _context.Set<TEntity>().Update(entity);
      return await _context.SaveChangesAsync();
    }

    public async Task<int> DeleteAsyncs(Func<TEntity, bool> predicate)
    {
      var entities = _context.Set<TEntity>().Where(predicate).ToList();
      _context.Set<TEntity>().RemoveRange(entities);
      return await _context.SaveChangesAsync();
    }

    public async Task<IDisposable> BeginTransactionAsync()
    {
      var transaction = await _context.Database.BeginTransactionAsync();
      return transaction;
    }

    public async Task<List<TEntity>> GetListAsync()
    {
      var dbSet = _context.Set<TEntity>();
      return await dbSet.ToListAsync();
    }

    public async Task<List<TEntity>> GetListAsync(Func<TEntity, bool> predicate)
    {
      var dbSet = _context.Set<TEntity>();
      return await Task.Run(() => dbSet.Where(predicate).ToList());
    }
  }
}
