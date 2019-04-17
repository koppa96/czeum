﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Czeum.Server.Services.EmailSender
{
    public interface IEmailService
    {
        Task SendConfirmationEmailAsync(string to, string token);
        Task SendPasswordResetEmailAsync(string to, string token);
    }
}
