﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.IO;

namespace LifestyleCommon
{
    public class MainConfig
    {
        private JObject m_jConfig = null;
        public string m_sDB_Name = "localhost";
        public int m_nDB_Port = 3306;
        public string m_sDB_User = "root";
        public string m_sDB_Pwd = "root";
        public string m_sIB_Host = "";
        public int m_nIB_Port = 4002;
        public int m_nIB_ID = 1;

        public MainConfig(string sFile)
        {
            m_jConfig = JObject.Parse(File.ReadAllText(sFile));
            var jDB = m_jConfig["database"];
            m_sDB_Name = (string)jDB["db_name"];
            m_nDB_Port = (int)jDB["port"];
            m_sDB_User = (string)jDB["user"];
            m_sDB_Pwd = (string)jDB["password"];
            var jIB = m_jConfig["ib"];
            m_sIB_Host = (string)jIB["host"];
            m_nIB_Port = (int)jIB["port"];
            m_nIB_ID = (int)jIB["client_id"];
        }
    }

    public class SymbolConfig
    {
        private JObject m_jConfig = null;
        public List<Symbol> m_lstSymbol = new List<Symbol>();

        public SymbolConfig(string sFile)
        {
            m_jConfig = JObject.Parse(File.ReadAllText(sFile));
            JArray jSymbols = (JArray)m_jConfig["symbols"];
            m_lstSymbol.Clear();
            foreach (var jSymbol in jSymbols)
            {
                Symbol symbol = new Symbol();
                symbol.m_sSymbol = (string)jSymbol["symbol"];
                symbol.m_sExchange = (string)jSymbol["exchange"];
                symbol.m_sSecurity = (string)jSymbol["security"];
                JArray jTimeFrames = (JArray)jSymbol["timeframes"];
                foreach (var jTimeFrame in jTimeFrames)
                {
                    symbol.m_lstTimeFrame.Add(new TimeFrame()
                    {
                        m_sName = (string)jTimeFrame["name"],
                        m_dtStart = TimeFrame.ConvertStartTime((string)jTimeFrame["start"])
                    });
                }
                m_lstSymbol.Add(symbol);
            }
        }
    }

    public class Symbol
    {
        public string m_sSymbol;
        public string m_sExchange;
        public string m_sSecurity;
        public List<TimeFrame> m_lstTimeFrame = new List<TimeFrame>();
    }

    public class TimeFrame
    {
        public string m_sName;
        public DateTime m_dtStart;

        public static DateTime ConvertStartTime(string sStart)
        {// not completed yet
            return new DateTime();
        }
    }
}
