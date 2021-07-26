using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifestyleCommon;

namespace LifestyleTrader
{
    class TFEngine
    {
        Symbol m_symbol = null;

        public TFEngine(Symbol symbol)
        {
        }

        public void PushTick(Tick tick)
        {
        }

        public void PushOhlc(Ohlc ohlc)
        {
        }

        public Ohlc GetOhlc(string sTF, int nShift = 0)
        {
            return null;
        }

        public List<string> TFList()
        {
            return null;
        }

        public int OhlcCount(string sTF)
        {
            return 0;
        }

        public DateTime GetTime()
        {
            return DateTime.Now;
        }

        public double Ask()
        {
            return 0;
        }

        public double Bid()
        {
            return 0;
        }
    }
}
