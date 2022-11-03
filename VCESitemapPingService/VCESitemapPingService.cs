using BusinessLogic;
using ServiceLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace VCESitemapPingService
{
    public partial class VCESitemapPingService : ServiceBase
    {
        #region Global Declarations
        LoggingHelper loggingHelper = new LoggingHelper();
        string className = "VCESitemapPingService";

        Timer timersitemap;
        bool verifysitemap = false;

        #endregion

        #region Constr
        public VCESitemapPingService()
        {
            InitializeComponent();
        }
        #endregion

        #region OnStart
        protected override void OnStart(string[] args)
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: OnStart - Begin");
            try
            {
                if (ConfigurationManager.AppSettings["SitemapInterval"] != null && ConfigurationManager.AppSettings["SitemapInterval"].ToString()!="0" && ConfigurationManager.AppSettings["SitemapInterval"].ToString() != "")
                {
                    initGeneratesitemap();
                    timersitemap.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Error, " Classname: " + className + " :: OnStart - error " + ex.Message);
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: OnStart - End");
        }
        #endregion

        #region convertMinutesToSeconds      
        private double convertMinutesToSeconds(float hours)
        {

            double retVal = 0;
            //interval to 1 minute (= 60,000 milliseconds)
            retVal = hours*60*60*1000;
            return retVal;
        }
        #endregion

        #region initGeneratesitemap
        public void initGeneratesitemap()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: initGeneratesitemap - begin");
            if (timersitemap == null)
            {
                timersitemap = new Timer();
                //Handle elapsed event
                timersitemap.Elapsed += new ElapsedEventHandler(Generatesitemap_OnElapsedTime);
                timersitemap.Interval = convertMinutesToSeconds(float.Parse(ConfigurationManager.AppSettings["SitemapInterval"].ToString()));
                //autoreset timer
                timersitemap.AutoReset = true;
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: initGeneratesitemap - end");
        }
        #endregion

        #region Generatesitemap_OnElapsedTime      
        private void Generatesitemap_OnElapsedTime(object source, ElapsedEventArgs e)
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: Generatesitemap_OnElapsedTime - begin");
            if (!verifysitemap)
            {
                verifysitemap = true;
                GenerateSiteMappingXML();
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: Generatesitemap_OnElapsedTime - end");
        }
        #endregion

        #region GenerateSiteMappingXML
        private void GenerateSiteMappingXML()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSiteMappingXML - begin");
            try
            {
                SiteMap objSiteMap = new SiteMap();
                objSiteMap.GenerateSitemapXML();
                verifysitemap = false;
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Error, " Classname: " + className + " :: GenerateSiteMappingXML - error " + ex.Message);
            }
            finally
            {
                verifysitemap = false;
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GenerateSiteMappingXML - end");
        }
        #endregion

        #region OnStop
        protected override void OnStop()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: OnStop - Begin");
            try
            {
                if (timersitemap != null)
                    timersitemap.Enabled = false;
                            
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Error, " Classname: " + className + " :: OnStop - error " + ex.Message);
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: OnStop - End");
        }
        #endregion
    }
}
