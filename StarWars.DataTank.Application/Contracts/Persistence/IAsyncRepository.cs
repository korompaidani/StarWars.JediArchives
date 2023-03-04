using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StarWars.DataTank.Application.Contracts.Persistence
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<T> GetByIdAsync(Guid id);
        Task<IReadOnlyList<T>> ListAllAsync();
        Task<T> AddAsync(T model);
        Task UpdateAsync(T model);
        Task DeleteAsync(T model);
    }
}