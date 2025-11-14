using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace UserManagement.Data;

public interface IDataContext
{
    Task<List<TEntity>> GetAllAsync<TEntity>() where TEntity : class;
    Task<List<TEntity>> GetAsync<TEntity>(Expression<Func<TEntity, bool>>? filter = null) where TEntity : class;
    Task<TEntity> GetByIDAsync<TEntity>(object id) where TEntity : class;
    Task CreateAsync<TEntity>(TEntity entity) where TEntity : class;
    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
    Task DeleteAsync<TEntity>(object id) where TEntity : class;
}
