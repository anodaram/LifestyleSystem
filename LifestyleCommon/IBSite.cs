using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LifestyleCommon
{
    public class IBSite
    {
        public void Init()
        {

        }

        public bool Connect(string sHost, int nPort, int nID)
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

        public bool RequestOrder(Symbol symbol, ORDER_COMMAND cmd, ref double dLots, ref double dPrice)
        {
            return true;
        }

        public double GetLots(Symbol symbol)
        {
            return 0;
        }

        public Tick GetRate(Symbol symbol)
        {
            return new Tick();
        }
    }
}
