using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using Npgsql;

namespace BusinessLogic
{
    /// <summary>
    /// Summary description for SmartDataReader.
    /// </summary>
    /// 

    public sealed class SmartDataReader
    {
        private DateTime defaultDate;
        public SmartDataReader(NpgsqlDataReader reader)
        {
            this.defaultDate = DateTime.MinValue;
            this.reader = reader;
        }

        public long GetInt64(String column)
        {
            long data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (long)0 : (long)reader[column];
            return data;
        }

        public int GetInt32(String column)
        {
            int data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (int)0 : (int)reader[column];
            return data;
        }

        public short GetInt16(String column)
        {
            short data = (reader.IsDBNull(reader.GetOrdinal(column))) ? (short)0 : (short)reader[column];
            return data;
        }
        public int GetTinyInt(String column)
        {

            //int data =(reader.IsDBNull(reader.GetOrdinal(column))) ? 0 : (int)reader[column];
            int data;

            if (!(reader[column].ToString() == null))
            {

                if (int.Parse(reader[column].ToString()) == 1)
                {
                    data = 1;
                }
                else
                    data = 0;

            }
            else
                data = 0;


            return data;
        }
        public float GetFloat(String column)
        {
            float data = (reader.IsDBNull(reader.GetOrdinal(column))) ? 0 : float.Parse(reader[column].ToString());
            return data;
        }

        public bool GetBoolean(String column)
        {
            bool data = (reader.IsDBNull(reader.GetOrdinal(column))) ? false : (bool)reader[column];
            return data;
        }

        public String GetString(String column)
        {
            String data = (reader.IsDBNull(reader.GetOrdinal(column))) ? string.Empty : reader[column].ToString();
            return data;
        }

        public DateTime GetDateTime(String column)
        {
            DateTime data = (reader.IsDBNull(reader.GetOrdinal(column))) ? defaultDate : (DateTime)reader[column];
            return data;
        }

        public byte[] GetBinary(String column)
        {
            byte[] data = (reader.IsDBNull(reader.GetOrdinal(column))) ? null : (byte[])reader[column];
            return data;
        }

        public bool Read()
        {
            return this.reader.Read();
        }
        private NpgsqlDataReader reader;
    }
}
