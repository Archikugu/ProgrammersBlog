﻿using ProgrammersBlog.Core.DataAccess.Abstract;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Abstract;

public interface IArticleRepository : IEntityRepository<Article>
{
}
