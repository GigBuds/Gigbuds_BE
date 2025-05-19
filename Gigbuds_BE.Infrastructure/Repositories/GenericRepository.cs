using AutoMapper;
using AutoMapper.QueryableExtensions;
using Gigbuds_BE.Application.Interfaces;
using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Application.Specifications;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Gigbuds_BE.Infrastructure.Repositories
{
    internal class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        // ===========================
        // === Fields & Props
        // ===========================

        private readonly GigbudsDbContext _dbContext;

        // ===========================
        // === Constructors
        // ===========================

        public GenericRepository(GigbudsDbContext context)
        {
            _dbContext = context;
        }

        // ===========================
        // === INSERT, UPDATE, DELETE
        // ===========================

        public T? Insert(T entity)
        {
            var addedEntity = _dbContext.Set<T>().Add(entity).Entity;
            return addedEntity;
        }

        public T? Update(T entityToUpdate)
        {
            _dbContext.Entry(entityToUpdate).State = EntityState.Modified;
            return entityToUpdate;
        }

        public T? Delete(T entityToDelete)
        {
            var deletedEntity = _dbContext.Set<T>().Remove(entityToDelete).Entity;
            return deletedEntity;
        }

        public T? Delete(object id)
        {
            var entityToDelete = _dbContext.Set<T>().Find(id);
            return entityToDelete == null ? null : Delete(entityToDelete);
        }

        public T? SoftDelete(T entityToDelete)
        {
            _dbContext.Entry(entityToDelete).State = EntityState.Modified;
            entityToDelete.IsEnabled = true;
            return entityToDelete;
        }
        public T? SoftDelete(object id)
        {
            var entityToDelete = _dbContext.Set<T>().Find(id);
            return entityToDelete == null ? null : SoftDelete(entityToDelete);
        }

        // ========================================
        // === GET queries with Specification
        // === Using with Include and .ThenInclude
        // ========================================

        public async Task<IReadOnlyList<T>> GetAllWithSpecificationAsync(ISpecification<T> specification, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await SpecificationQueryBuilder<T>.BuildQuery(query, specification).ToListAsync();
        }

        public async Task<T?> GetBySpecificationAsync(ISpecification<T> specification, bool asNoTracking = true)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            if (asNoTracking)
            {
                query = query.AsNoTracking();
            }
            return await SpecificationQueryBuilder<T>.BuildQuery(query, specification).FirstOrDefaultAsync();
        }

        public async Task<int> CountAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            return await SpecificationQueryBuilder<T>.BuildCountQuery(query, specification).CountAsync();
        }

        // ===========================================
        // === GET queries Projection with AutoMapper
        // === Using with Profile and Dto
        // ===========================================
        public async Task<TDto?> GetBySpecificationProjectedAsync<TDto>(ISpecification<T> specification, IConfigurationProvider mapperConfig)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            return await SpecificationQueryBuilder<T>.BuildQuery(query, specification)
                .ProjectTo<TDto>(mapperConfig)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<TDto>> GetAllWithSpecificationProjectedAsync<TDto>(ISpecification<T> specification, IConfigurationProvider mapperConfig)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            return await SpecificationQueryBuilder<T>.BuildQuery(query, specification)
                .ProjectTo<TDto>(mapperConfig)
                .ToListAsync();
        }

        // ===========================================
        // === GET queries with GROUP BY 
        // ===========================================

        public async Task<IReadOnlyList<T>> GetAllGroupByAsync(ISpecification<T> specification)
        {
            IQueryable<T> query = _dbContext.Set<T>().AsQueryable();
            return await SpecificationQueryBuilder<T>.BuildGroupByQuery(query, specification).ToListAsync();
        }

    }
}
