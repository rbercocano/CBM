using System.Collections.Generic;

namespace Charcutarie.Core.SMTP
{
    public interface IEmailManager
    {
        void SendEmail(List<string> to, string body, string subject, bool isHtml = false);
        void SendRegistrationEmail(string account, string username, string to, string company, int customerTypeId, string socialIdentifier);
    }
}