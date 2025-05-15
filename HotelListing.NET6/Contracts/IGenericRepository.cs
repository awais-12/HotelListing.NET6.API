using System;
using System.Linq.Expressions;
using HotelListing.NET6.Models;

namespace HotelListing.NET6.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameter);
        Task<T> AddAsync(T entity);
        Task<T> GetAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(T entity);
        Task<bool> ExistAsync(int id);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        IQueryable<T> FindByConditionWithTracking(Expression<Func<T, bool>> expression);
    }
}

