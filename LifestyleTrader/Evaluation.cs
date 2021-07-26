using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LifestyleCommon;

namespace LifestyleTrader
{
    class Evaluation
    {
        bool m_bUpdated = true;
        double m_dBalance;
        double m_dAsset;
        double m_dLots;
        List<Position> m_lstPos = new List<Position>();
        double m_dMaxProfit;
        double m_dMDD;

        public Evaluation()
        {
            Init();
        }

        public void Init()
        {
            m_dBalance = 0;
            m_dAsset = 0;
            m_dLots = 0;
            m_lstPos.Clear();
            m_dMaxProfit = 0;
            m_dMDD = 0;
        }

        public void RequestOrder(ORDER_COMMAND cmd, double dLots, double dPrice)
        {
            m_bUpdated = true;
            if (cmd == ORDER_COMMAND.BUY || cmd == ORDER_COMMAND.SELLCLOSE)
            {
                m_dLots += dLots;
                m_dAsset -= dLots * dPrice;
            }
            if (cmd == ORDER_COMMAND.SELL || cmd == ORDER_COMMAND.BUYCLOSE)
            {
                m_dLots -= dLots;
                m_dAsset += dLots * dPrice;
            }
            if (Math.Abs(m_dLots) < Global.EPS)
            {
                m_dLots = 0;
                m_dBalance = m_dAsset;
                m_dMaxProfit = Math.Max(m_dBalance, m_dMaxProfit);
                m_dMDD = Math.Max(m_dMDD, m_dMaxProfit - m_dBalance);
            }
            m_lstPos.Add(new Position()
            {
                eCmd = cmd,
                dLots = dLots,
                dPrice = dPrice
            });
        }

        public double Lots()
        {
            return m_dLots;
        }

        public bool IsUpdated(bool bCheck = true)
        {
            bool bRlt = m_bUpdated;
            if (bCheck) m_bUpdated = false;
            return bRlt;
        }

        private struct Position
        {
            public ORDER_COMMAND eCmd;
            public double dLots;
            public double dPrice;
        }
    }
}
