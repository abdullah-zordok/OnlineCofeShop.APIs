using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities;
using Talabat.Core.Specifications;

namespace Talabat.Repository
{
    internal class SpecificationsEvaluator<TEntity> where TEntity : BaseEntity
    {
        public static IQueryable<TEntity> GetQuery(IQueryable<TEntity> inputQuery,ISpecifications<TEntity> spec)
        {
            var query=inputQuery.AsQueryable();
            if(spec.Criteria is not null)
            query = query.Where(spec.Criteria);
            if(spec.OrderBy is not null)
                query = query.OrderBy(spec.OrderBy);
            else if(spec.OrderByDesc is not null)
                query=query.OrderByDescending(spec.OrderByDesc);
            if(spec.IsPaginationEnabled)
                query=query.Skip(spec.Skip).Take(spec.Take);
            query=spec.Includes.Aggregate(query,(currentQuery,includeExpression)=>currentQuery.Include(includeExpression));
            return query;

            
        }
    }
}
