using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Core.DataAccess.Concrete.EntityFramework;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Repositories;

public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryRepository
{
    public EfCategoryRepository(DbContext context) : base(context)
    {
    }

    public async Task<Category> GetById(int categoryId)
    {
        return await ProgrammersBlogContext.Categories.SingleOrDefaultAsync(c => c.Id == categoryId);
    }

    private ProgrammersBlogContext ProgrammersBlogContext
    {
        get { return _context as ProgrammersBlogContext; }
    }
}
