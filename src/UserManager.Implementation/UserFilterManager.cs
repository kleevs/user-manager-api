﻿using System.Linq;
using UserManager.Model;

namespace UserManager.Implementation
{
    public class UserFilterManager : IFilterManager<IFilter, IUserFilterable>
    {
        public IQueryable<IUserFilterable> Apply(IFilter filter, IQueryable<IUserFilterable> query)
        {
            if (filter == null)
            {
                return query;
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(_ => _.IsActive == filter.IsActive);
            }

            if (filter.Id.HasValue)
            {
                query = query.Where(_ => _.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Email))
            {
                query = query.Where(_ => _.Email.StartsWith(filter.Email));
            }

            return query.OrderBy(_ => _.Id);
        }
    }
}