namespace MedicalProj.Data.Contracts
{
    public interface IGenericRepository<TEntity, TKey> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(bool isTrackable = false);
        Task<IEnumerable<TEntity>> GetAllAsync(Specification<TEntity> specifications);
        Task<TEntity?> GetByIdAsync(TKey id);
        Task<TEntity?> GetByIdAsync(Specification<TEntity> specifications);
        Task AddAsync(TEntity entity);
        Task AddAllAsync(IEnumerable<TEntity> entities);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void DeleteAll(IEnumerable<TEntity> entities);
        Task<bool> ExistsAsync(TKey id);
    }
}
