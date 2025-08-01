﻿using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Repositories;

namespace ProgrammersBlog.Core.DataAccess.Concrete;

public class UnitOfWork : IUnitOfWork
{
    private readonly ProgrammersBlogContext _context;
    private EfArticleRepository _articleRepository;
    private EfCategoryRepository _categoryRepository;
    private EfCommentRepository _commentRepository;

    public UnitOfWork(ProgrammersBlogContext context)
    {
        _context = context;
    }
    public IArticleRepository Articles => _articleRepository = _articleRepository ?? new EfArticleRepository(_context);

    public ICategoryRepository Categories => _categoryRepository ??= new EfCategoryRepository(_context);

    public ICommentRepository Comments => _commentRepository ??= new EfCommentRepository(_context);

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public async ValueTask DisposeAsync()
    {
        await _context.DisposeAsync();
    }
}
