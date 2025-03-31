namespace ProgrammersBlog.Core.Entities.Abstract;

public abstract class EntityBase
{
    public virtual int Id { get; set; }
    public virtual DateTime CreatedDate { get; set; } = new DateTime(2025, 3, 31);
    public virtual DateTime ModifiedDate { get; set; } = new DateTime(2025, 3, 31);
    public virtual bool IsDeleted { get; set; } = false;
    public virtual bool IsActive { get; set; } = true;
    public virtual string CreatedByName { get; set; } = "Admin";
    public virtual string ModifiedByName { get; set; } = "Admin";
    public virtual string Note { get; set; }
}
