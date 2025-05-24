using AutoMapper;
using Gigbuds_BE.Domain.Entities;

namespace Gigbuds_BE.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        // GET
        Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification, bool asNoTracking = true);
        Task<T?> GetBySpecificationAsync(ISpecification<T> specification, bool asNoTracking = true);

        // GET support Projection with Automapper for better performance
        Task<TDto?> GetBySpecificationProjectedAsync<TDto>(ISpecification<T> specification, IConfigurationProvider mapperConfig);
        Task<IReadOnlyList<TDto>> GetAllWithSpecificationProjectedAsync<TDto>(ISpecification<T> specification, IConfigurationProvider mapperConfig);

        // INSERT, DELETE, UPDATE
        public T Insert(T entity);
        public T? Update(T entityToUpdate);
        public T? Delete(T entityToDelete);
        public T? Delete(object id);
        public T? SoftDelete(T entityToDelete);
        public T? SoftDelete(object id);
    }
}
