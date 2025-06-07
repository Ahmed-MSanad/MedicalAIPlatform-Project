using System.Collections.Concurrent;
using MedicalProj.Data.Contracts;
using Persistence.Data;
using Persistence.Repositories;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MedicalDbContext dbContext;
        private ConcurrentDictionary<string, object> repositories;

        public UnitOfWork(MedicalDbContext _dbContext)
        {
            dbContext = _dbContext;
            repositories = new();
        }
        public async Task<int> SaveChangesAsync()
            => await dbContext.SaveChangesAsync();
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class
            => (IGenericRepository<TEntity, TKey>)repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, TKey>(dbContext));
    }
}
