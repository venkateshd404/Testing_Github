using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ServiceLogic
{
    public class EmailNotifications
    {
        #region Global Declarations
        LoggingHelper loggingHelper = new LoggingHelper();
        string className = "EmailNotifications";
        #endregion    

        #region SendSMTPErrorReport
        public bool SendSMTPErrorReport(string msg)
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: SendSMTPErrorReport - Begin");
            bool retVal = false;
            string toAddress = string.Empty;
            string Cc = string.Empty;
            MailMessage mail = null;           
            try
            {               
                toAddress = ConfigurationManager.AppSettings["NotifyEmails"].ToString().Replace(" ", "");
                Cc = ConfigurationManager.AppSettings["Cc"].ToString().Replace(" ", "");
                if (toAddress != null && toAddress != "")
                {
                    toAddress = toAddress.Replace(" ", "");
                    mail = new MailMessage();
                    SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpHost"].ToString());
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["SmtpFrom"].ToString());
                    mail.To.Add(toAddress);
                    mail.CC.Add(Cc); 
                    mail.Subject = "VCE Sitemap Ping Error Notification";                    
                    mail.Body = "Hi," + "<br/>" + "<br/>"+ msg + "<br/><br/> Thankyou,<br/> Support Team";
                    mail.IsBodyHtml = true;             
                    SmtpServer.Port = Convert.ToInt16(ConfigurationManager.AppSettings["SmtpPort"].ToString());
                    SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["SmtpUserName"].ToString(), ConfigurationManager.AppSettings["SmtpPassword"].ToString());
                    SmtpServer.EnableSsl = Convert.ToBoolean(ConfigurationManager.AppSettings["SmtpEnableSSL"].ToString());
                    SmtpServer.Send(mail);
                    mail.Dispose();
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                mail.Dispose();
                loggingHelper.Log(LoggingLevels.Error, " Classname: " + className + " :: SendSMTPErrorReport - error " + ex.Message);
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: SendSMTPErrorReport - End");
            return retVal;
        }
        #endregion
           
    }
}
