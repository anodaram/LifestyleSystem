using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace LifestyleTrader
{
    class Global
    {
        // size
        public const int MAX_LOG_DISPLAY_LENGTH = 2048;

        // file path
        public static string LOG_DIR = Directory.GetCurrentDirectory() + "\\log\\";
    }

    enum RUN_MODE
    {
        BACKTEST = 0,
        REAL_TRADE = 1
    }
}
