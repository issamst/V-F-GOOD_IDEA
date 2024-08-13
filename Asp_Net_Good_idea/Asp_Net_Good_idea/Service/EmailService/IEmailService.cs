using Asp_Net_Good_idea.Models.EmailModel;

namespace Asp_Net_Good_idea.UtilityService.EmailService
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);


    }
}
