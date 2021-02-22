using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Net.Mail;
using Lean.Authorization.Users;
using Lean.Email.Model;
using Lean.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Lean.Email
{
    public class EmailService : LeanServiceBase, IEmailService, ITransientDependency
    {
        private readonly IEmailBodyRenderer _emailRenderer;
        private readonly IEmailSender _emailSender;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IRepository<User, long> _userRepository;

        public EmailService(
            IEmailBodyRenderer emailRenderer,
            IEmailSender emailSender,
            ITimeZoneService timeZoneService,
            IRepository<User, long> userRepository)
        {
            _emailRenderer = emailRenderer;
            _emailSender = emailSender;
            _timeZoneService = timeZoneService;
            _userRepository = userRepository;
        }

        public async Task SendTestEmail()
        {
            var to = "reply.to.this@email.com";
            var body = await _emailRenderer.Render(
                "ContactUs", 
                new ContactUsModel { Message = "Contact us message", ReplyTo = to, Title = "Contact Us" });
            await _emailSender.SendAsync(new MailMessage
            {
                To = { to },
                Subject = "Contact Us (TEST)",
                Body = body,
                IsBodyHtml = true
            });
        }
    }
}
