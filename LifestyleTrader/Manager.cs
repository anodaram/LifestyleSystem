﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using LifestyleCommon;

namespace LifestyleTrader
{
    class Manager
    {
        public static Form1 g_mainForm = null;
        private static StreamWriter g_fileLog = null;
        private static MainConfig g_mainConfig = null;
        private static SymbolConfig g_symbolConfig = null;
        private static Strategy g_strategy = null;
        private static ChartCon g_chart = new ChartCon();
        public static TradeHistory g_tradeHistory = new TradeHistory();
        public static RUN_MODE g_eMode = RUN_MODE.NONE;
        public static MySQL g_database = null;
        private static IBSite g_broker = null;
        private static Thread g_mainThread = null;
        private static bool g_bRunning = false;
        private static DateTime g_dtLastDisplayState = new DateTime();

        public static void Init(Form1 form)
        {
            g_mainForm = form;
            PutLog("Inited");
            g_mainConfig = new MainConfig(Global.MAIN_CONFIG);
            g_symbolConfig = new SymbolConfig(Global.SYMBOL_CONFIG);
            Global.OnLog = PutLog;
            PutLog("Loading Config success");
        }

        public static void PutLog(string sLog)
        {
            if (g_fileLog == null)
            {
                string sLogDir = Global.LOG_TRADER_DIR;
                if (!Directory.Exists(sLogDir))
                {
                    Directory.CreateDirectory(sLogDir);
                }
                g_fileLog = new StreamWriter(sLogDir + DateTime.Now.ToString("yyyyMMddHH") + ".txt", true);
                g_fileLog.AutoFlush = true;
            }
            lock (g_fileLog)
            {
                string sLogEx = string.Format("[{0}] {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), sLog);
                g_fileLog.WriteLine(sLogEx);
                g_mainForm.DisplayLog(sLogEx);
            }
        }

        public static void Start(RUN_MODE eMode, string sSymbol, DateTime dtStart, DateTime dtEnd)
        {
            if (g_database == null)
            {// init database
                PutLog("Initializing Database ...");
                g_database = new MySQL(g_mainConfig.m_sDB_Name, g_mainConfig.m_nDB_Port,
                    g_mainConfig.m_sDB_User, g_mainConfig.m_sDB_Pwd);
                PutLog("Database init success");
            }
            PutLog(string.Format("Start({0},{1},{2},{3})", eMode, sSymbol, dtStart, dtEnd));
            Symbol symbol = g_symbolConfig.FindSymbol(sSymbol);
            if (symbol == null)
            {
                PutLog("Can't find such symbol");
                return;
            }

            g_strategy = new Strategy(symbol, Newtonsoft.Json.Linq.JObject.Parse(File.ReadAllText(Global.STRATEGY_CONFIG)));

            (g_mainThread = new Thread(() =>
            {
                g_bRunning = true;
                if (eMode == RUN_MODE.BACKTEST)
                {
                    runBacktest(dtStart, dtEnd);
                }
                else if (eMode == RUN_MODE.REAL_TRADE)
                {
                    runRealTrade();
                }
                else if (eMode == RUN_MODE.MERGE_MODE)
                {
                    runBacktest(dtStart, dtEnd);
                    runRealTrade();
                }
            })).Start();
        }

        public static void Stop()
        {
            g_eMode = RUN_MODE.NONE;
            if (g_mainThread != null)
            {
                g_bRunning = false;
                Thread.Sleep(1000);
                try
                {
                    g_mainThread.Abort();
                }
                catch { }
            }
        }

        public static bool ConnectChart()
        {
            return g_chart.Connect();
        }

        private static void runBacktest(DateTime dtStart, DateTime dtEnd)
        {
            var lstOhlc = g_database.Get(g_strategy.SymbolEx(), dtStart, dtEnd);
            int nTot = lstOhlc.Count;
            int nCur = 0;
            foreach (var ohlc in lstOhlc)
            {
                if (!g_bRunning) break;
                g_strategy.PushOhlc(ohlc);
                g_strategy.OnTick();
                nCur++;
            }
        }

        private static void runRealTrade()
        {
            while (g_bRunning)
            {
                Tick tick = g_broker.GetRate(g_strategy.symbol());
                g_strategy.PushTick(tick);
                Thread.Sleep(100);
            }
        }
    }
}
