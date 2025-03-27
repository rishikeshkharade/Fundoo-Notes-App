using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace CommonLayer.Models
{
    public class SendingMailToUser
    {
        public string SendingMail(string ToEmail, string Token)
        {
            string FromEmail = "rishikeshkharade19@gmail.com";
            MailMessage mailMessage = new MailMessage(FromEmail, ToEmail);
            //string resetUrl = $"https://4200/reset-password?token={Token}";
            string MailBody = "Token Generated : " + Token;
            mailMessage.Subject = "Token Generated for Forget Password";
            mailMessage.Body = MailBody.ToString();
            mailMessage.BodyEncoding = Encoding.UTF8;
            mailMessage.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            NetworkCredential credential = new NetworkCredential("rishikeshkharade19@gmail.com", "bgeb kmva qomg xgfa");

            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = credential;

            smtp.Send(mailMessage);
            return ToEmail;
        } 
    }
}
