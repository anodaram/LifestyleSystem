﻿using System;
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

        // constants
        public const double EPS = 1e-5;
        
        // Util functions
        public static DateTime UnixSecondsToDateTime(long unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp);
            return dtDateTime;
        }

        public static long UnixDateTimeToSeconds(DateTime dt)
        {
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan diff = dt - origin;
            return (long)diff.TotalSeconds;
        }
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
