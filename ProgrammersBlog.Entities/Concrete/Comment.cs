﻿using ProgrammersBlog.Core.Entities.Abstract;

namespace ProgrammersBlog.Entities.Concrete;

public class Comment : EntityBase, IEntity
{
    //public override bool IsActive { get; set; } = false;
    public string Text { get; set; }
    public string? Note { get; set; }
    public int ArticleId { get; set; }
    public Article Article { get; set; }
}
