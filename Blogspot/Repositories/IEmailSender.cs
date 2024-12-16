﻿namespace Blogspot.Repositories
{
    public interface IEmailSender
    {

        Task<bool> EmailSendAsync(string email,string subject,string message);

    }
}
