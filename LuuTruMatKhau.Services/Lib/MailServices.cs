using LuuTruMatKhau.Model;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace LuuTruMatKhau.Services.Lib
{
    public static class MailServices
    {
        private static string GetHtmlMessage(string msg)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"<div>Your verifycation code: {msg}</div>");
            return builder.ToString();
        }
        public static bool SendVerifyEmail(ConfigMailModel config)
        {
            try
            {

                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress(config.MailFrom);
                message.To.Add(new MailAddress(config.MailTo));
                message.Subject = config.Subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = GetHtmlMessage(config.Message);
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(config.MailFrom, PasswordServices.DecryptString(config.KeyDecrypt, config.Password));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                return true;
            }
            catch
            {
                return false;
            }

        }
    }
}
