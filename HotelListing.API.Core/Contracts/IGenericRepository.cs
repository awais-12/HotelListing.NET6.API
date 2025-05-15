using System;
using System.Linq.Expressions;
using HHotelListing.API.Core.Models;
using HotelListing.API.Core.Models;

namespace HotelListing.API.Core.Contracts
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
     
        Task<TResult> GetAsync<TResult>(int? id);
        Task<List<TResult>> GetAllAsync<TResult>();
        Task<TResult> AddAsync<TSource, TResult>(TSource source);
        Task UpdateAsync<TSource>(int id, TSource source);
       
    }
}

