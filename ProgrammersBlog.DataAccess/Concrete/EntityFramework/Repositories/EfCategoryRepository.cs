﻿using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Core.DataAccess.Concrete.EntityFramework;
using ProgrammersBlog.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Repositories;

public class EfCategoryRepository : EfEntityRepositoryBase<Category>, ICategoryRepository
{
    public EfCategoryRepository(DbContext context) : base(context)
    {
    }
}
