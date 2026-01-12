using ShopTARgv24.Core.Dto;

namespace ShopTARgv24.Core.ServiceInterface
{
    public interface IEmailServices
    {
        void SendEmail(EmailDto dto);
        void SendEmailToken(EmailTokenDto dto, string token);
    }
}