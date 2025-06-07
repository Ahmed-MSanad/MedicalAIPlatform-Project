using System.Linq.Expressions;

namespace MedicalProj.Data.Contracts
{
    public abstract class Specification<T> where T : class
    {
        protected Specification(Expression<Func<T, bool>> creteria)
        {
            Creteria = creteria;
        }
        public Expression<Func<T, bool>> Creteria { get; private set; }
        public List<Expression<Func<T, object>>> Includes { get; } = new();
        public Expression<Func<T, object>> OrderBy { get; private set; } = null!;
        public Expression<Func<T, object>> OrderByDescending { get; private set; } = null!;

        protected void AddInclude(Expression<Func<T, object>> include)
            => Includes.Add(include);

        protected void setOrderBy(Expression<Func<T, object>> expression)
            => OrderBy = expression;

        protected void setOrderByDescending(Expression<Func<T, object>> expression)
            => OrderByDescending = expression;
    }
}
