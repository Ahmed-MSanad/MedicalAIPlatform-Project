using MedicalProj.Data.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public static class SpecificationEvaluator
    {
        public static IQueryable<T> GetQuery<T>(IQueryable<T> baseQuery, Specification<T> specifications) where T : class
        {
            var query = baseQuery;

            if (specifications.Creteria != null)
                query = query.Where(specifications.Creteria);

            query = specifications.Includes.Aggregate(query, (currentQuery, include) => currentQuery.Include(include));

            return query;
        }
    }
}
