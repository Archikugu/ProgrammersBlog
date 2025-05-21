using AutoMapper;
using ProgrammersBlog.DataAccess.Abstract;

namespace ProgrammersBlog.Business.Concrete;

public class ManagerBase
{
    protected IUnitOfWork UnitOfWork { get; }
    protected IMapper Mapper { get; }

    public ManagerBase(IUnitOfWork unitOfWork, IMapper mapper)
    {
        UnitOfWork = unitOfWork;
        Mapper = mapper;
    }
}
