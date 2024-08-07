﻿using CCMS.BE.Interfaces;
using CCMS.Common.Dto.Request.Mail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace CCMS.BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailingController : ControllerBase
    {
        private readonly IMailingService _mailingService;

        public MailingController(IMailingService mailingService)
        {
            _mailingService = mailingService;
        }

        [HttpPost("send")]
        public async Task<IActionResult> SendMail([FromForm] AddMail dto)
        {
            await _mailingService.SendEmailAsync(dto.ToEmail, dto.Subject, dto.Body, dto.Attachments);
            return Ok();
        }

        //[HttpPost("welcome")]
        //public async Task<IActionResult> SendWelcomeEmail([FromBody] AddMail dto)
        //{
        //    var filePath = $"{Directory.GetCurrentDirectory()}\\Templates\\EmailTemplate.html";
        //    var str = new StreamReader(filePath);

        //    var mailText = str.ReadToEnd();
        //    str.Close();

        //    mailText = mailText.Replace("[username]", dto.UserName).Replace("[email]", dto.Email);

        //    await _mailingService.SendEmailAsync(dto.Email, "Welcome to our channel", mailText);
        //    return Ok();
        //}
    }
}
