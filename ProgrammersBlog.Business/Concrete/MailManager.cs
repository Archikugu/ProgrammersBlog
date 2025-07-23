using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using ProgrammersBlog.Business.Abstract;
using ProgrammersBlog.Core.Utilities.Results.Abstract;
using ProgrammersBlog.Core.Utilities.Results.ComplexTypes;
using ProgrammersBlog.Core.Utilities.Results.Concrete;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos.EmailDtos;

namespace ProgrammersBlog.Business.Concrete;
public class MailManager : IMailService
{
    private readonly SmtpSettings _smtpSettings;

    public MailManager(IOptions<SmtpSettings> smtpSettings)
    {
        _smtpSettings = smtpSettings.Value;
    }

    public IResult Send(EmailSendDto emailSendDto)
    {
        MailMessage message = new MailMessage
        {
            From = new MailAddress(_smtpSettings.SenderEmail),
            To = { new MailAddress(emailSendDto.Email) },
            Subject = emailSendDto.Subject,
            IsBodyHtml = true,
            Body = $"Sender Name: {emailSendDto.Name}, Sender Mail Address: {emailSendDto.Email}\n{emailSendDto.Message}"
        };
        SmtpClient smtpClient = new SmtpClient
        {
            Host = _smtpSettings.Server,
            Port = _smtpSettings.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };
        smtpClient.Send(message);
        return new Result(ResultStatus.Success, "Email sent successfully.");
    }

    public IResult SendContactEmail(EmailSendDto emailSendDto)
    {
        MailMessage message = new MailMessage
        {
            From = new MailAddress(_smtpSettings.SenderEmail),
            To = { new MailAddress("engokhangok@outlook.com") },
            Subject = emailSendDto.Subject,
            IsBodyHtml = true,
            Body = $"Sender Name: {emailSendDto.Name}, Sender Mail Address: {emailSendDto.Email}\n{emailSendDto.Message}"
        };
        SmtpClient smtpClient = new SmtpClient
        {
            Host = _smtpSettings.Server,
            Port = _smtpSettings.Port,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
            DeliveryMethod = SmtpDeliveryMethod.Network
        };
        smtpClient.Send(message);
        return new Result(ResultStatus.Success, "Email sent successfully.");
    }
}
