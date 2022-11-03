using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using log4net;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]
namespace BusinessLogic
{
    /// <summary>
    ///*****************************************************************************
    ///Namespace        : Schedule Handler
    ///Class			: LoggingHelper,LoggingLevels
    ///Description		: for logs
    ///$Author: $		: Venkatesh +91 9705963669
    ///List of pages this class navigates to : 
    ///$Date: $ 	    : 23/08/2018(MM/DD/YYYY)	
    ///$Modtime: $ 	    Date and time of last modification
    ///$Revision: $ 
    ///Modified by		     
    ///Date Modified	
    ///Reason for modification
    ///
    ///
    ///*****************************************************************************
    /// </summary>
    /// 
    public class LoggingHelper
    {
        private ILog log;

        public LoggingHelper()
        {
            log = LogManager.GetLogger(this.GetType());
        }

        public void Log(LoggingLevels level, string message)
        {
            switch (level)
            {
                case LoggingLevels.Debug:
                    log.Debug(message);
                    break;
                case LoggingLevels.Error:
                    log.Error(message);
                    break;
                case LoggingLevels.Info:
                    log.Info(message);
                    break;
                case LoggingLevels.Warn:
                    log.Warn(message);
                    break;
                case LoggingLevels.Fatal:
                    log.Fatal(message);
                    break;
                default:
                    break;
            }
        }

        public void Log(LoggingLevels level, string message, Exception exception)
        {
            switch (level)
            {
                case LoggingLevels.Debug:
                    log.Debug(message, exception);
                    break;
                case LoggingLevels.Error:
                    log.Error(message, exception);
                    break;
                case LoggingLevels.Info:
                    log.Info(message, exception);
                    break;
                case LoggingLevels.Warn:
                    log.Warn(message, exception);
                    break;
                case LoggingLevels.Fatal:
                    log.Fatal(message, exception);
                    break;
                default:
                    break;
            }
        }
    }

    public enum LoggingLevels
    {
        Warn,
        Info,
        Debug,
        Error,
        Fatal
    }
}
