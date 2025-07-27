﻿using System;
using System.Linq.Expressions;
using LinqKit;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Core.DataAccess.Abstract;
using ProgrammersBlog.Core.Entities.Abstract;

namespace ProgrammersBlog.Core.DataAccess.Concrete.EntityFramework;

public class EfEntityRepositoryBase<TEntity> : IEntityRepository<TEntity>
    where TEntity : class, IEntity, new()
{
    protected readonly DbContext _context;

    public EfEntityRepositoryBase(DbContext context)
    {
        _context = context;
        //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking; // Disable change tracking for better performance
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
        return entity;
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>().AnyAsync(predicate);
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null)
    {
        return await (predicate == null ? _context.Set<TEntity>().CountAsync() : _context.Set<TEntity>().CountAsync(predicate));
    }

    public async Task DeleteAsync(TEntity entity)
    {
        //await Task.Run(() => { _context.Set<TEntity>().Remove(entity); });

        _context.Set<TEntity>().Remove(entity);
    }

    public async Task<IList<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (predicate != null)
        {
            query = query.Where(predicate);
        }
        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<IList<TEntity>> GetAllAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (predicates != null && predicates.Any())
        {
            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }
        }

        if (includeProperties != null && includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        query = query.Where(predicate);

        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.AsNoTracking().SingleOrDefaultAsync();
    }

    public async Task<TEntity?> GetAsyncV2(IList<Expression<Func<TEntity, bool>>> predicates, IList<Expression<Func<TEntity, object>>> includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();

        if (predicates != null && predicates.Any())
        {
            foreach (var predicate in predicates)
            {
                query = query.Where(predicate);
            }
        }

        if (includeProperties != null && includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }
        return await query.AsNoTracking().SingleOrDefaultAsync();
    }

    public async Task<IList<TEntity>> SearchAsync(IList<Expression<Func<TEntity, bool>>> predicates, params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (predicates.Any())
        {
            var predicateChain = PredicateBuilder.New<TEntity>();
            foreach (var predicate in predicates)
            {
                // predicate1 && predicate2 && predicate3 && predicateN
                // predicate1 || predicate2 || predicate3 || predicateN
                predicateChain.Or(predicate);
            }

            query = query.Where(predicateChain);
        }

        if (includeProperties.Any())
        {
            foreach (var includeProperty in includeProperties)
            {
                query = query.Include(includeProperty);
            }
        }

        return await query.AsNoTracking().ToListAsync();
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        _context.Set<TEntity>().Update(entity);
        return entity;
    }
}
