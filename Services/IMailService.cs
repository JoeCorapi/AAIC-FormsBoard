﻿using System.Collections.Generic;

namespace FormsBoard.Services
{
    public interface IMailService
    {
        void SendEmail(string Subject, string HTMLBody, IEnumerable<string> EmailList);

        void SendEmailAttachment(string Subject, string HTMLBody, byte[] data, string fileName, string mediaType, IEnumerable<string> EmailList);
    }
}
