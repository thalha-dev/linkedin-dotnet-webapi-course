using System.Linq.Expressions;

namespace HPlusSport.API.Models
{
    public static class IQueryableExtensions
    {
        public static IQueryable<TEntity> OrderByCustom<TEntity>(this IQueryable<TEntity> items, string sortBy, string sortOrder)
        {
            var type = typeof(TEntity);
            var pe = Expression.Parameter(type, "t");
            var property = type.GetProperty(sortBy);
            var mae = Expression.MakeMemberAccess(pe, property);
            var lambda = Expression.Lambda(mae, pe);
            var result = Expression.Call(
                typeof(Queryable),
                sortOrder == "desc" ? "OrderByDescending" : "OrderBy",
                new Type[] { type, property.PropertyType },
                items.Expression,
                Expression.Quote(lambda));

            return items.Provider.CreateQuery<TEntity>(result);
        }
    }
}

