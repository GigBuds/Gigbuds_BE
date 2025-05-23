using Gigbuds_BE.Application.Interfaces.Repositories;
using Gigbuds_BE.Domain.Entities;
using Gigbuds_BE.Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Gigbuds_BE.Infrastructure.Persistence
{
    internal class UnitOfWork : IUnitOfWork
    {
        // ===================================
        // === Fields & Prop
        // ===================================

        private readonly GigbudsDbContext _dbContext;

        private readonly ILoggerFactory _loggerFactory;

        private readonly ConcurrentDictionary<string, object> _repositoryDictionary;

        // ===================================
        // === Constructors
        // ===================================

        public UnitOfWork(GigbudsDbContext dbContext, ILoggerFactory loggerFactory)
        {
            _dbContext = dbContext;
            _loggerFactory = loggerFactory;
            _repositoryDictionary = new();
        }


        // ===================================
        // === Methods 
        // ===================================

        /// <summary>
        ///     Dispose object
        /// </summary>
        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task<int> CompleteAsync() => await _dbContext.SaveChangesAsync();

        public IGenericRepository<T> Repository<T>() where T : BaseEntity
        {
            var typeEntityName = typeof(T).Name;

            var repoInstanceTypeT = _repositoryDictionary.GetOrAdd(typeEntityName,
                valueFactory: _ =>
                {
                    var repoType = typeof(GenericRepository<T>);
                    //var repoLogger = _loggerFactory.CreateLogger<GenericRepository<T>>();

                    //var repoInstance = Activator.CreateInstance(repoType, _dbContext, repoLogger);
                    var repoInstance = Activator.CreateInstance(repoType, _dbContext);

                    return repoInstance!;
                });

            return (IGenericRepository<T>)repoInstanceTypeT;
        }
    }
}
