using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LifestyleCommon
{
    public class Global
    {
        public static Action<string> OnLog = new Action<string>((x) => { });

        // file path
        public static string LOG_TRADER_DIR = Directory.GetCurrentDirectory() + "\\logs\\log_trader\\";
        public static string MAIN_CONFIG = Directory.GetCurrentDirectory() + "\\config\\main.json";
        public static string SYMBOL_CONFIG = Directory.GetCurrentDirectory() + "\\config\\symbols.json";
        public static string STRATEGY_CONFIG = Directory.GetCurrentDirectory() + "\\config\\strategy.json";
    }

    public class Ohlc
    {
        public long time;
        public double open;
        public double high;
        public double low;
        public double close;
        public double open_ask;
        public double high_ask;
        public double low_ask;
        public double close_ask;
    }

    public class Tick
    {
        public long time;
        public double ask;
        public double bid;
    }


    public enum ORDER_COMMAND
    {
        NONE = 0,
        BUY = 1,
        SELL = 2,
        BUYCLOSE = 3,
        SELLCLOSE = 4
    }
}
