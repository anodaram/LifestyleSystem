using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifestyleCommon;

namespace LifestyleTrader
{
    enum Pattern
    {
        None = 0,
        Up = 1,
        Down = 2,
        Dojo = 3,
        InRange = 4, InBody = 5,
        A1 = 6, A2 = 7,
        B1 = 8, B2 = 9,
        C1 = 10, C2 = 11,
        D1 = 12, D2 = 13,
        E1 = 14, E2 = 15,
        F1 = 16, F2 = 17, F3 = 18, F4 = 19,
        G1 = 20, G2 = 21, G3 = 22,
        H1 = 23, H2 = 24, H3 = 25
    }

    class Patterns
    {
    }

    class PersistentOhlc
    {
        List<Ohlc> m_lstOhlc = new List<Ohlc>();

        public bool Append(Ohlc ohlc)
        {
            if (m_lstOhlc.Count > 0 && m_lstOhlc[m_lstOhlc.Count - 1].time == ohlc.time) return false;
            m_lstOhlc.Add(ohlc);
            return true;
        }

        public int Count()
        {
            return m_lstOhlc.Count;
        }

        public Ohlc GetShift(int nShift)
        {
            return m_lstOhlc[m_lstOhlc.Count - nShift];
        }
    }
}
