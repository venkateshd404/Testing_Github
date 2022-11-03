using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
namespace ServiceLogic
{
    public class SiteMap
    {
        LoggingHelper loggingHelper = new LoggingHelper();
        EmailNotifications objnotifcation = new EmailNotifications();
        DataAccess objdataccess = new DataAccess();
        string className = "SiteMap";
        public void GenerateSitemapXML()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML - Begin");
            List<PublicationDTO> lstPublicationDTO = new List<PublicationDTO>();
            string msg = string.Empty;
            try
            {
                try
                {
                    lstPublicationDTO = objdataccess.GetPublicationDetails();
                }
                catch (Exception ex)
                {
                    msg = "System failed to connect to database. Please see the following exception message for more details. <br/><br/>" + ex.Message;                   
                    throw ex;
                }
                try
                {
                    XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
                    XNamespace xsiNs = "https://www.w3.org/1999/xhtml";
                    string fileName = ConfigurationManager.AppSettings["FileName"].ToString();
                    if (lstPublicationDTO.Count > 0)
                    {
                        XDocument sitemap = new XDocument(
                             new XDeclaration("1.0", "utf-8", "yes"),
                             new XElement(xmlns + "urlset",
                             new XAttribute("xmlns", xmlns),
                            new XAttribute(XNamespace.Xmlns + "xsi", xsiNs),

                         from item in lstPublicationDTO
                         select CreateItemElement(item)
                         )
                    );
                        string SiteMapFilePath = ConfigurationManager.AppSettings["SiteMapFilePath"].ToString();
                        if (!Directory.Exists(SiteMapFilePath))
                        {
                            Directory.CreateDirectory(SiteMapFilePath);
                        }
                        sitemap.Save(SiteMapFilePath + fileName);
                        try
                        {
                            if (File.Exists(SiteMapFilePath + fileName))
                            {
                                string sitemapUrl = ConfigurationManager.AppSettings["SITEMAP_URL"].ToString();

                                var request = WebRequest.Create(sitemapUrl);
                                request.Method = "GET";

                                var webResponse = request.GetResponse();
                                var webStream = webResponse.GetResponseStream();

                                var reader = new StreamReader(webStream);
                                var data = reader.ReadToEnd();

                                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: SiteMap Xml Submitted response :: " + data);
                            }
                            else
                            {
                                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: SiteMap Xml not Found");
                            }

                        }
                        catch (Exception ex)
                        {
                            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: sitemap submition failed - ERROR - " + ex.Message);
                            msg = "System failed to ping your sitemap xml to Google. Please see the following exception message for more details. <br/><br/>" + ex.Message;
                            objnotifcation.SendSMTPErrorReport(msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML - ERROR - " + ex.Message);
                    msg = "System failed to generate sitemap xml. Please see the following exception message for more details. <br/>" + ex.Message;
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML final- ERROR - " + ex.Message);
                objnotifcation.SendSMTPErrorReport(msg);
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML - End");
        }
        public XElement CreateItemElement(PublicationDTO item)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace xsiNs = "https://www.w3.org/1999/xhtml";

            XElement itemElement = null;
            try
            {               
                string LAST_MODIFY = DateTime.Now.ToString("yyyy-MM-dd");
                string CHANGE_FREQ = ConfigurationManager.AppSettings["CHANGE_FREQENCY"].ToString();
                string TOP_PRIORITY = ConfigurationManager.AppSettings["TOP_PRIORITY"].ToString();
                string DOMAIN = ConfigurationManager.AppSettings["DOMAIN"].ToString();
                DOMAIN = DOMAIN + "contentdetail?contentid=" + item.productContentId + "&contentname=" + item.productContentName;
                itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", DOMAIN));
                itemElement.Add(new XElement(xmlns + "lastmod", LAST_MODIFY));
                itemElement.Add(new XElement(xmlns + "changefreq", CHANGE_FREQ));
                itemElement.Add(new XElement(xmlns + "priority", TOP_PRIORITY));
                
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: CreateItemElement - ERROR - " + ex.Message);
                throw ex;
            }
            return itemElement;
        }

        public XElement CreateItemElement1(string item)
        {
            XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
            XNamespace xsiNs = "https://www.w3.org/1999/xhtml";

            XElement itemElement = null;
            try
            {
                string LAST_MODIFY = DateTime.Now.ToString("yyyy-MM-dd");
                string CHANGE_FREQ = ConfigurationManager.AppSettings["CHANGE_FREQENCY"].ToString();
                string TOP_PRIORITY = ConfigurationManager.AppSettings["TOP_PRIORITY"].ToString();
                string DOMAIN = ConfigurationManager.AppSettings["DOMAIN"].ToString();
                
                itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", item));
                itemElement.Add(new XElement(xmlns + "lastmod", LAST_MODIFY));
                itemElement.Add(new XElement(xmlns + "changefreq", CHANGE_FREQ));
                itemElement.Add(new XElement(xmlns + "priority", TOP_PRIORITY));

            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: CreateItemElement - ERROR - " + ex.Message);
                throw ex;
            }
            return itemElement;
        }

        public void GenerateSitemapXML1()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML1 - Begin");
            List<string> lsturls = new List<string>();
            string msg = string.Empty;
            try
            {
                try
                {
                    lsturls = objdataccess.GetPublicationDetails1();
                }
                catch (Exception ex)
                {
                    msg = "System failed to connect to database. Please see the following exception message for more details. <br/><br/>" + ex.Message;
                    throw ex;
                }
                try
                {
                    XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
                    XNamespace xsiNs = "https://www.w3.org/1999/xhtml";
                    string fileName = ConfigurationManager.AppSettings["FileName"].ToString();
                    if (lsturls.Count > 0)
                    {
                        XDocument sitemap = new XDocument(
                             new XDeclaration("1.0", "utf-8", "yes"),
                             new XElement(xmlns + "urlset",
                             new XAttribute("xmlns", xmlns),
                            new XAttribute(XNamespace.Xmlns + "xsi", xsiNs),

                         from item in lsturls
                         select CreateItemElement1(item)
                         )
                    );
                        string SiteMapFilePath = ConfigurationManager.AppSettings["SiteMapFilePath"].ToString();
                        if (!Directory.Exists(SiteMapFilePath))
                        {
                            Directory.CreateDirectory(SiteMapFilePath);
                        }
                        sitemap.Save(SiteMapFilePath + fileName);
                        try
                        {
                            if (File.Exists(SiteMapFilePath + fileName))
                            {
                                string sitemapUrl = ConfigurationManager.AppSettings["SITEMAP_URL"].ToString();

                                var request = WebRequest.Create(sitemapUrl);
                                request.Method = "GET";

                                var webResponse = request.GetResponse();
                                var webStream = webResponse.GetResponseStream();

                                var reader = new StreamReader(webStream);
                                var data = reader.ReadToEnd();

                                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: SiteMap Xml Submitted response :: " + data);
                            }
                            else
                            {
                                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: SiteMap Xml not Found");
                            }

                        }
                        catch (Exception ex)
                        {
                            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: sitemap submition failed - ERROR - " + ex.Message);
                            msg = "System failed to ping your sitemap xml to Google. Please see the following exception message for more details. <br/><br/>" + ex.Message;
                            objnotifcation.SendSMTPErrorReport(msg);
                        }
                    }
                }
                catch (Exception ex)
                {
                    loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML - ERROR - " + ex.Message);
                    msg = "System failed to generate sitemap xml. Please see the following exception message for more details. <br/>" + ex.Message;
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML final- ERROR - " + ex.Message);
                objnotifcation.SendSMTPErrorReport(msg);
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSitemapXML - End");
        }
    }
}
