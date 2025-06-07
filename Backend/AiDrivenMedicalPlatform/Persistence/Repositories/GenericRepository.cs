using MedicalProj.Data.Contracts;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Persistence.Repositories
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        private readonly MedicalDbContext dbContext;

        public GenericRepository(MedicalDbContext _dbContext)
        {
            dbContext = _dbContext;
        }
        public async Task AddAsync(TEntity entity)
            => await dbContext.Set<TEntity>().AddAsync(entity);

        public async Task AddAllAsync(IEnumerable<TEntity> entities)
            => await dbContext.AddRangeAsync(entities);


        public async Task<bool> ExistsAsync(TKey id)
            => await dbContext.Set<TEntity>().FindAsync(id) != null;

        public async Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false)
        {
            if(isTrackable)
                return await dbContext.Set<TEntity>().ToListAsync();

            return await dbContext.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByIdAsync(TKey id)
            => await dbContext.Set<TEntity>().FindAsync(id);
        public void Delete(TEntity entity)
            => dbContext.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity)
            => dbContext.Set<TEntity>().Update(entity);


        public async Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specifications)
            => await ApplyQuery(specifications).ToListAsync();
        public async Task<TEntity?> GetByIdAsync(Specification<TEntity> specifications)
            => await ApplyQuery(specifications).FirstOrDefaultAsync();
        public IQueryable<TEntity> ApplyQuery(Specification<TEntity> specifications)
            => SpecificationEvaluator.GetQuery(dbContext.Set<TEntity>(), specifications);

        public void DeleteAll(IEnumerable<TEntity> entities)
            => dbContext.Set<TEntity>().RemoveRange(entities);
    }
}
