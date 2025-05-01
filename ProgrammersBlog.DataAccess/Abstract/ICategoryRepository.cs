using ProgrammersBlog.Core.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Abstract;

public interface ICategoryRepository : IEntityRepository<Category>
{
    Task<Category> GetById(int categoryId);
}
