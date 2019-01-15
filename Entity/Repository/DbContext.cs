﻿using Microsoft.EntityFrameworkCore;
using Model;

namespace Entity
{
    public partial class DbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public virtual DbSet<User> User { get; set; }

        public DbContext(DbContextOptions<DbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToSchema("UserManager");
            });
        }
    }
}