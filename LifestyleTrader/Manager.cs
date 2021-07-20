using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LifestyleTrader
{
    class Manager
    {
        public static Form1 g_mainForm = null;
        static StreamWriter g_fileLog = null;

        public static void Init(Form1 form)
        {
            g_mainForm = form;
            PutLog("Inited");
        }

        public static void PutLog(string sLog)
        {
            if (g_fileLog == null)
            {
                string sLogDir = Global.LOG_DIR;
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

        }

        public static void Stop()
        {

        }

        public static void ConnectChart()
        {

        }

        private static void loadConfig()
        {

        }
    }
}
