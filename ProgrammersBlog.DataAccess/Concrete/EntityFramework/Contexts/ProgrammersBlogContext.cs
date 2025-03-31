using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProgrammersBlog.DataAccess.Concrete.EntityFramework.Mappings;
using ProgrammersBlog.Entities.Concrete;

namespace ProgrammersBlog.DataAccess.Concrete.EntityFramework.Contexts;

public class ProgrammersBlogContext : DbContext
{
    public ProgrammersBlogContext(DbContextOptions<ProgrammersBlogContext> options)
        : base(options)
    {

    }

    public DbSet<Article> Articles { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ArticleMapping());
        modelBuilder.ApplyConfiguration(new CategoryMapping());
        modelBuilder.ApplyConfiguration(new CommentMapping());
        modelBuilder.ApplyConfiguration(new RoleMapping());
        modelBuilder.ApplyConfiguration(new UserMapping());
    }
}
