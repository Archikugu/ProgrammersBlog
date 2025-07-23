using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Entities.Dtos.EmailDtos;

namespace ProgrammersBlog.Business.Abstract;

public interface IMailService
{
    IResult Send(EmailSendDto emailSendDto);
    IResult SendContactEmail(EmailSendDto emailSendDto);
}
