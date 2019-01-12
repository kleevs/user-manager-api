using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace Entity
{
    public static class RelationalEntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder<TEntity> ToSchema<TEntity>(
            this EntityTypeBuilder<TEntity> entityTypeBuilder,
            string schema
        ) where TEntity : class
        {
            return entityTypeBuilder.ToTable(typeof(TEntity).Name, schema);
        }
    }
}
