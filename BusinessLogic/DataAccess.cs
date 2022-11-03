using Npgsql;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class DataAccess
    {
        LoggingHelper loggingHelper = new LoggingHelper();       
        string className = "DataAccess";
        string strConnString = null;
        public DataAccess()
        {
            strConnString = ConfigurationManager.AppSettings["ConnectionString"].ToString();      
        }
        public List<PublicationDTO> GetPublicationDetails()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GetPublicationDetails - Begin");
            NpgsqlConnection objcon = new NpgsqlConnection(strConnString);
            List<PublicationDTO> lstPublicationDTO = new List<PublicationDTO>();
            try
            {
                objcon.Open();
                string requestQuery = "select * from public.fngetpublicationdetailsforsitemap()";
                NpgsqlCommand command = new NpgsqlCommand(requestQuery, objcon);

                NpgsqlDataReader rdr = command.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(rdr);
                while (smartReader.Read())
                {
                    PublicationDTO objPublicationDTO = new PublicationDTO();
                    objPublicationDTO.productContentId = smartReader.GetInt64("productContentId");
                    objPublicationDTO.productContentName = smartReader.GetString("productContentName");
                    lstPublicationDTO.Add(objPublicationDTO);
                }
                objcon.Close();
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Error, " Classname: " + className + " :: GetPublicationDetails - error " + ex.Message);                
                throw ex;
            }
            finally
            {
                objcon.Close();
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GetPublicationDetails - End");
            return lstPublicationDTO;
        }
        public List<string> GetPublicationDetails1()
        {
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GetPublicationDetails1 - Begin");
            NpgsqlConnection objcon = new NpgsqlConnection(strConnString);
            List<PublicationDTO> lstPublicationDTO = new List<PublicationDTO>();
            List<string> lsturls = new List<string>();
            try
            {
                objcon.Open();
                string requestQuery = "select \"URL\" from \"Latest_Pri\"";
                NpgsqlCommand command = new NpgsqlCommand(requestQuery, objcon);

                NpgsqlDataReader rdr = command.ExecuteReader();
                SmartDataReader smartReader = new SmartDataReader(rdr);
                while (smartReader.Read())
                {
                    string url = smartReader.GetString("url");
                    lsturls.Add(url);
                    
                }
                objcon.Close();
            }
            catch (Exception ex)
            {
                loggingHelper.Log(LoggingLevels.Error, " Classname: " + className + " :: GetPublicationDetails1 - error " + ex.Message);
                throw ex;
            }
            finally
            {
                objcon.Close();
            }
            loggingHelper.Log(LoggingLevels.Info, " Classname: " + className + " :: GetPublicationDetails1 - End");
            return lsturls;
        }
    }
}
