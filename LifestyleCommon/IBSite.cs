using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifestyleCommon
{
    class IBSite
    {
        public void Init()
        {

        }

        public bool Connect(string sHost, string sPort, string sID)
        {
            return true;
        }

        public void Disconnect()
        {

        }

        public void Subscribe(List<Symbol> lstSymbol)
        {

        }

        public List<Ohlc> GetHistoricalData(Symbol symbol, DateTime dtStart, DateTime dtEnd)
        {
            return null;
        }

        public bool RequestOrder(ORDER_COMMAND cmd, Symbol symbol, ref double dLots, ref double dPrice)
        {
            return true;
        }

        public Tick GetRate(Symbol symbol)
        {
            return new Tick();
        }
    }
}
