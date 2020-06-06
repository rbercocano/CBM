using System.Collections.Generic;

namespace Charcutarie.Core.SMTP
{
    public interface IEmailManager
    {
        void SendEmail(List<string> to, string body, string subject, bool isHtml = false);
    }
}