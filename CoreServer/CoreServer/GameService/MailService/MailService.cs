using CoreCommon;
using CoreCommon.GameLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CoreServer.GameService
{
    /// <summary>
    /// 邮件服务
    /// </summary>
    public class MailService : Singleton<MailService>
    {
        /// <summary>
        /// 获取email 登录地址
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public string GetEmailLoginUrl(string email)
        {
            if ((email == string.Empty) || (email.IndexOf("@") <= 0))
            {
                return string.Empty;
            }
            int index = email.IndexOf("@");
            email = "http://mail." + email.Substring(index + 1);
            return email;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="mailFrom">发送者</param>
        /// <param name="mailList">邮件地址列表</param>
        /// <param name="hostIP">主机ip</param>
        /// <returns></returns>
        public void SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailList, string hostIP)
        {
            try
            {
                MailMessage message = new MailMessage()
                {
                    IsBodyHtml = false,
                    Subject = mailSubjct,
                    Body = mailBody,
                    From = new MailAddress(mailFrom),
                };
                for (int i = 0; i < mailList.Count; i++)
                {
                    message.To.Add(mailList[i]);
                }
                new SmtpClient
                {
                    UseDefaultCredentials = false,
                    DeliveryMethod = SmtpDeliveryMethod.PickupDirectoryFromIis,
                    Host = hostIP,
                    Port = (char)0x19
                }.SendMailAsync(message);
            }
            catch (Exception e)
            {
                LoggerHelper.Debug(e);
                return;
            }
        }

        /// <summary>
        /// 发送邮件(需要登录)
        /// </summary>
        /// <param name="mailSubjct"></param>
        /// <param name="mailBody"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailAddress"></param>
        /// <param name="HostIP"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string hostIP, string username, string password)
        {
            try
            {
                bool flag = false;
                string mess = SendMail(mailSubjct, mailBody, mailFrom, mailAddress, hostIP, 0x19, username, password, false, string.Empty, out flag);
                return flag;
            }
            catch (Exception ex)
            {
                LoggerHelper.Debug(ex);
                return false;
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubjct">邮件主题</param>
        /// <param name="mailBody">邮件正文</param>
        /// <param name="mailFrom">发送者</param>
        /// <param name="mailAddress">接收地址列表</param>
        /// <param name="HostIP">主机ip</param>
        /// <param name="filename">附件名</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="ssl">加密类型</param>
        /// <returns></returns>
        public string SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string HostIP, string filename, string username, string password, bool ssl)
        {
            string str = "";
            try
            {
                MailMessage message = new MailMessage
                {
                    IsBodyHtml = false,
                    Subject = mailSubjct,
                    Body = mailBody,

                    From = new MailAddress(mailFrom)
                };
                for (int i = 0; i < mailAddress.Count; i++)
                {
                    message.To.Add(mailAddress[i]);
                }
                if (System.IO.File.Exists(filename))
                {
                    message.Attachments.Add(new Attachment(filename));
                }
                SmtpClient client = new SmtpClient
                {
                    EnableSsl = ssl,
                    UseDefaultCredentials = false
                };
                NetworkCredential credential = new NetworkCredential(username, password);
                client.Credentials = credential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = HostIP;
                client.Port = 0x19;
                client.Send(message);
            }
            catch (Exception exception)
            {
                str = exception.Message;
            }
            return str;
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="mailSubjct"></param>
        /// <param name="mailBody"></param>
        /// <param name="mailFrom"></param>
        /// <param name="mailAddress"></param>
        /// <param name="HostIP"></param>
        /// <param name="port"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="ssl"></param>
        /// <param name="replyTo"></param>
        /// <param name="sendOK"></param>
        /// <returns></returns>

        public string SendMail(string mailSubjct, string mailBody, string mailFrom, List<string> mailAddress, string HostIP, int port, string username, string password, bool ssl, string replyTo, out bool sendOK)
        {
            sendOK = true;
            string str = "";
            try
            {
                MailMessage message = new MailMessage
                {
                    IsBodyHtml = false,
                    Subject = mailSubjct,
                    Body = mailBody,
                    From = new MailAddress(mailFrom)
                };
                if (replyTo != string.Empty)
                {
                    MailAddress address = new MailAddress(replyTo);
                    message.ReplyTo = address;
                }
                Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
                for (int i = 0; i < mailAddress.Count; i++)
                {
                    if (regex.IsMatch(mailAddress[i]))
                    {
                        message.To.Add(mailAddress[i]);
                    }
                }
                if (message.To.Count == 0)
                {
                    return string.Empty;
                }
                SmtpClient client = new SmtpClient
                {
                    EnableSsl = ssl,
                    UseDefaultCredentials = false
                };
                NetworkCredential credential = new NetworkCredential(username, password);
                client.Credentials = credential;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Host = HostIP;
                client.Port = port;
                client.Send(message);
            }
            catch (Exception exception)
            {
                str = exception.Message;
                sendOK = false;
            }
            return str;
        }
    }
}