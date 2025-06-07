
namespace MedicalProj.Data.Contracts
{
    public interface IUnitOfWork
    {
        public Task<int> SaveChangesAsync();
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : class;
    }
}
