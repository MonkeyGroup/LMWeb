using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace LM.Utility
{
    public class EmailHelper
    {
        private static readonly string SmtpServer = ConfigurationManager.AppSettings["mail_server"];
        private static readonly int SmtpServerPort = int.Parse(ConfigurationManager.AppSettings["mail_server_port"]);
        private static readonly string SmtpAccount = ConfigurationManager.AppSettings["mail_account"];
        private static readonly string SmtpPassword = ConfigurationManager.AppSettings["mail_password"];

        /// <summary>
        /// 发送邮件，可带附件
        /// </summary>
        /// <param name="sendToEmail">接收方邮箱</param>
        /// <param name="subject">邮件标题</param>
        /// <param name="content">邮件内容</param>
        /// <param name="attachName">附件名称</param>
        /// <param name="attachFilePath">附件路径</param>
        public static void SendMail(string sendToEmail, string subject, string content, string attachName, string attachFilePath)
        {
            var client = new SmtpClient(SmtpServer, SmtpServerPort)
            {
                Credentials = new NetworkCredential(SmtpAccount, SmtpPassword)
            };

            var attach = new Attachment(attachFilePath) { Name = attachName };
            var message = new MailMessage(SmtpAccount, sendToEmail, subject, content);
            message.Attachments.Add(attach);
            
            client.Send(message);
        }
    }
}