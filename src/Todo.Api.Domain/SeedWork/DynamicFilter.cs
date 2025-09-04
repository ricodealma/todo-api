using System.Linq.Expressions;

namespace Multipay.Receivable.Microservice.Api.Domain.SeedWork
{
    public static class DynamicFilter
    {
        public static Expression<Func<T, bool>> GenerateFilter<T>(List<Expression<Func<T, bool>>> filters)
        {
            Expression<Func<T, bool>>? where = null;

            foreach (Expression<Func<T, bool>> expression in filters)
            {
                if (where == null)
                {
                    where = expression;
                }
                else
                {
                    InvocationExpression invokedExpr = Expression.Invoke(expression, where.Parameters[0]);
                    where = Expression.Lambda<Func<T, bool>>(
                    Expression.AndAlso(where.Body, invokedExpr), where.Parameters);
                }
            }

            return where!;
        }
    }
}
