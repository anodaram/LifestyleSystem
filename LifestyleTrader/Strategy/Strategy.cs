using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifestyleCommon;
using Newtonsoft.Json.Linq;

namespace LifestyleTrader
{
    class Strategy
    {
        private TFEngine m_TFEngine = null;
        private Symbol m_symbol = null;

        public Strategy(Symbol symbol, JObject jStrategy)
        {
            m_symbol = symbol;
            m_TFEngine = new TFEngine(symbol);
        }

        public string SymbolEx()
        {
            return m_symbol.m_sSymbol;
        }

        public void PushTick(Tick tick)
        {
            m_TFEngine.PushTick(tick);
        }

        public void PushOhlc(Ohlc ohlc)
        {
            m_TFEngine.PushOhlc(ohlc);
        }
    }
}
