﻿using System;
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
        private Dictionary<Tuple<string, Pattern>, PersistentOhlc> m_dicPersistentOHLC = new Dictionary<Tuple<string, Pattern>, PersistentOhlc>();
        private double m_dDefaultRate = 0;
        private Dictionary<Tuple<string, string>, bool> m_dicStates = new Dictionary<Tuple<string, string>, bool>();
        private List<ORDER_COMMAND> m_lstSignal = new List<ORDER_COMMAND>();
        private JArray m_jStateFormula = null;
        private double ex_dLots = 1.0;
        private Evaluation m_evaluation = new Evaluation();

        public Strategy(Symbol symbol, JObject jStrategy)
        {
            m_symbol = symbol;
            m_TFEngine = new TFEngine(symbol, Manager.g_symbolConfig.m_lstTF);
            m_jStateFormula = (JArray)jStrategy["state_formula"];
        }

        public string SymbolEx()
        {
            return m_symbol.m_sSymbol;
        }

        public Symbol symbol()
        {
            return m_symbol;
        }

        public void PushTick(Tick tick)
        {
            m_TFEngine.PushTick(tick);
            
            // should set about default_rate
        }

        public void PushOhlc(Ohlc ohlc)
        {
            m_TFEngine.PushOhlc(ohlc);
        }

        public void OnTick()
        {
            initState();
            calcPattern();
            calcState_dfs(m_jStateFormula);

            ORDER_COMMAND cmd = openedCmd();
            double dLots = ex_dLots;
            double dPrice = 0;

            if (cmd == ORDER_COMMAND.NONE && m_lstSignal.Contains(ORDER_COMMAND.BUY))
            {
                dPrice = m_TFEngine.Ask();
                requestOrder(m_symbol, ORDER_COMMAND.BUY, ref dLots, ref dPrice);
            }
            else if (cmd == ORDER_COMMAND.NONE && m_lstSignal.Contains(ORDER_COMMAND.SELL))
            {
                dPrice = m_TFEngine.Bid();
                requestOrder(m_symbol, ORDER_COMMAND.SELL, ref dLots, ref dPrice);
            }
            else if (cmd == ORDER_COMMAND.BUY && m_lstSignal.Contains(ORDER_COMMAND.BUYCLOSE))
            {
                dPrice = m_TFEngine.Bid();
                requestOrder(m_symbol, ORDER_COMMAND.SELL, ref dLots, ref dPrice);
            }
            else if (cmd == ORDER_COMMAND.SELL && m_lstSignal.Contains(ORDER_COMMAND.SELLCLOSE))
            {
                dPrice = m_TFEngine.Ask();
                requestOrder(m_symbol, ORDER_COMMAND.BUY, ref dLots, ref dPrice);
            }
        }

        private void calcPattern()
        {
            foreach (var pattern in (Pattern[])Enum.GetValues(typeof(Pattern)))
            {
                foreach (var sTF in m_TFEngine.TFList())
                {
                    if (check(sTF, pattern))
                    {
                        var key = Tuple.Create(sTF, pattern);
                        if (!m_dicPersistentOHLC.ContainsKey(key)) m_dicPersistentOHLC[key] = new PersistentOhlc();
                        m_dicPersistentOHLC[key].Append(m_TFEngine.GetOhlc(sTF));
                    }
                }
            }
        }

        private bool check(string sTF, string sPattern, int nShift = 0)
        {
            Pattern pattern = Pattern.None;
            try
            {
                pattern = (Pattern)Enum.Parse(typeof(Pattern), sPattern, true);
            }
            catch (Exception e)
            {
                return false;
            }
            return check(sTF, pattern, nShift);
        }

        private bool check(string sTF, Pattern pattern, int nShift = 0)
        { // timeframe [M1, M2, M3, ..., M5, ...]
            // check("M5", "D2", 4)
            if (!m_TFEngine.TFList().Contains(sTF) || m_TFEngine.OhlcCount(sTF) < nShift)
            {
                return false;
            }
            if (pattern == Pattern.Up)
                return rate(sTF, 'C', nShift) > rate(sTF, 'O', nShift);
            if (pattern == Pattern.Down)
                return rate(sTF, 'C', nShift) < rate(sTF, 'O', nShift);
            if (pattern == Pattern.Dojo)
                return rate(sTF, 'C', nShift) == rate(sTF, 'O', nShift);
            if (pattern == Pattern.InBody)
                return (rate(sTF, 'C', nShift) - rate(sTF, 'O', nShift + 1)) * (rate(sTF, 'C', nShift) - rate(sTF, 'C', nShift + 1)) <= 0;
            if (pattern == Pattern.InRange)
                return (rate(sTF, 'C', nShift) - rate(sTF, 'H', nShift + 1)) * (rate(sTF, 'C', nShift) - rate(sTF, 'L', nShift + 1)) <= 0;
            if (pattern == Pattern.A1)
                return check(sTF, Pattern.Up, nShift + 1) && check(sTF, Pattern.Down, nShift);
            if (pattern == Pattern.A2)
                return check(sTF, Pattern.Down, nShift + 1) && check(sTF, Pattern.Up, nShift);
            if (pattern == Pattern.B1)
                return check(sTF, Pattern.Up, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && rate(sTF, 'C', nShift) > rate(sTF, 'H', nShift + 1);
            if (pattern == Pattern.B2)
                return check(sTF, Pattern.Down, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && rate(sTF, 'C', nShift) < rate(sTF, 'L', nShift + 1);
            if (pattern == Pattern.C1)
                return check(sTF, Pattern.Up, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.C2)
                return check(sTF, Pattern.Down, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.D1)
                return check(sTF, Pattern.B1, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.D2)
                return check(sTF, Pattern.B1, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && check(sTF, Pattern.InBody, nShift);
            if (pattern == Pattern.E1)
                return check(sTF, Pattern.B2, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.E2)
                return check(sTF, Pattern.B2, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && check(sTF, Pattern.InBody, nShift);
            if (pattern == Pattern.F1)
                return check(sTF, Pattern.A1, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && !check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.F2)
                return check(sTF, Pattern.A1, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.F3)
                return check(sTF, Pattern.A1, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.F4)
                return check(sTF, Pattern.A1, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && !check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.G1)
                return check(sTF, Pattern.A2, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && !check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.G2)
                return check(sTF, Pattern.A2, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.G3)
                return check(sTF, Pattern.A2, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && !check(sTF, Pattern.InBody, nShift) && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.H1)
                return check(sTF, Pattern.A2, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && !check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.H2)
                return check(sTF, Pattern.A2, nShift + 1) && check(sTF, Pattern.Down, nShift)
                    && check(sTF, Pattern.InRange, nShift);
            if (pattern == Pattern.H3)
                return check(sTF, Pattern.A2, nShift + 1) && check(sTF, Pattern.Up, nShift)
                    && check(sTF, Pattern.InRange, nShift);

            return false;
        }

        private double rate(string sTF, char c, int nShift)
        {
            if (!m_TFEngine.TFList().Contains(sTF) || m_TFEngine.OhlcCount(sTF) < nShift)
            {
                return m_dDefaultRate;
            }
            Ohlc ohlc = m_TFEngine.GetOhlc(sTF, nShift);
            if (ohlc == null) return m_dDefaultRate;
            if (c == 'O') return ohlc.open;
            if (c == 'H') return ohlc.high;
            if (c == 'L') return ohlc.low;
            if (c == 'C') return ohlc.close;
            return m_dDefaultRate;
        }

        protected DateTime getTime()
        {
            return m_TFEngine.GetTime();
        }

        private void calcState_dfs(JArray j)
        {
            if (j == null) return;
            foreach (var jChild in j)
            {
                bool bFlag = true;
                foreach (var p in ((JObject)jChild))
                {
                    if (p.Key == "set")
                    {
                        string[] sWords = ((string)p.Value).Split(' ');
                        setState(sWords[0], sWords[1]);
                    }
                    else if (p.Key == "Time")
                    {
                        string[] sWords = ((string)p.Value).Split(' ');
                        int nHourSt = int.Parse(sWords[0].Split(':')[0]);
                        int nMinSt = int.Parse(sWords[0].Split(':')[1]);
                        int nHourEn = int.Parse(sWords[1].Split(':')[0]);
                        int nMinEn = int.Parse(sWords[1].Split(':')[1]);
                        int nTotMinSt = nHourSt * 60 + nMinSt;
                        int nTotMinEn = nHourEn * 60 + nMinEn;
                        int nTotMinCur = getTime().Hour * 60 + getTime().Minute;
                        bFlag &= (nTotMinSt <= nTotMinCur) && (nTotMinCur < nTotMinEn);
                    }
                    else if (p.Key == "DayOfWeek")
                    {
                        bFlag &= (getTime().DayOfWeek.ToString()) == (string)p.Value;
                    }
                    else if (p.Key == "cmp")
                    {
                        double dLeft = pastRate((string)p.Value[0], (string)p.Value[1], ((string)p.Value[2])[0], (string)p.Value[3]);
                        double dRight = pastRate((string)p.Value[5], (string)p.Value[6], ((string)p.Value[7])[0], (string)p.Value[8]);
                        string cmp = (string)p.Value[4];
                        if (cmp == ">") bFlag &= dLeft > dRight;
                        else if (cmp == "<") bFlag &= dLeft < dRight;
                        else if (cmp == ">=") bFlag &= dLeft >= dRight;
                        else if (cmp == "<=") bFlag &= dLeft <= dRight;
                        else if (cmp == "==" || cmp == "=") bFlag &= dLeft == dRight;
                    }
                    else if (p.Key == "order")
                    {
                        setOrder((string)p.Value);
                        //Manager.PutLog("Set order : " + p.Value);
                    }
                    else if (p.Key == "children")
                    {
                        calcState_dfs((JArray)p.Value);
                    }
                    else
                    {
                        bFlag &= checkState(p.Key, (string)p.Value);
                    }
                    if (!bFlag) break;
                }
            }
        }

        private void initState()
        {
            m_dicStates.Clear();
            m_lstSignal.Clear();
        }

        private void setState(string sKey, string sValue)
        {
            m_dicStates[Tuple.Create(sKey, sValue)] = true;
        }

        private void setOrder(string sCmd)
        {
            string[] sWords = sCmd.Split(' ');
            m_lstSignal.Add((ORDER_COMMAND)Enum.Parse(typeof(ORDER_COMMAND), sWords[0]));
            double dLots = double.Parse(sWords[1]);
        }

        private bool checkState(string sKey, string sValue)
        {
            try
            {
                if (check(sKey, sValue)) return true;
            }
            catch { }
            return m_dicStates.ContainsKey(Tuple.Create(sKey, sValue));
        }

        private double pastRate(string sTF, string sPattern, char c, string sShift)
        {
            int nShift = sShift.Length < 1 ? 0 : int.Parse(sShift);
            if (sTF.Length == 0) // added at 2021-06-24
            {
                try
                {
                    return double.Parse(sPattern);
                }
                catch { }
            }
            if (sPattern.Length < 1) return this.rate(sTF, c, nShift);
            if (nShift == 0) return 0;
            try
            {
                var key = Tuple.Create(sTF, (Pattern)Enum.Parse(typeof(Pattern), sPattern, true));
                if (!m_dicPersistentOHLC.ContainsKey(key)) return 0;
                if (m_dicPersistentOHLC[key].Count() < nShift) return 0;
                Ohlc rate = m_dicPersistentOHLC[key].GetShift(nShift);
                if (c == 'O') return rate.open;
                if (c == 'H') return rate.high;
                if (c == 'L') return rate.low;
                if (c == 'C') return rate.close;
            }
            catch { return 0; }
            return 0;
        }

        private ORDER_COMMAND openedCmd()
        {
            double dLots = getLots(m_symbol);
            if (dLots > Global.EPS) return ORDER_COMMAND.BUY;
            if (dLots < -Global.EPS) return ORDER_COMMAND.SELL;
            return ORDER_COMMAND.NONE;
        }

        private double getLots(Symbol symbol)
        {
            if (Manager.g_eMode == RUN_MODE.BACKTEST)
            {
                return m_evaluation.Lots();
            }
            return Manager.g_broker.GetLots(symbol);
        }

        private bool requestOrder(Symbol symbol, ORDER_COMMAND cmd, ref double dLots, ref double dPrice)
        {
            Manager.PutLog(string.Format("requestOrder({0},{1},{2},{3})",
                symbol.m_sSymbol, cmd, dLots, dPrice));
            bool bRlt = true;
            if (Manager.g_eMode == RUN_MODE.REAL_TRADE)
            {
                bRlt = Manager.g_broker.RequestOrder(symbol, cmd, ref dLots, ref dPrice);
            }
            m_evaluation.RequestOrder(symbol.m_sSymbol, cmd, dLots, dPrice);
            return bRlt;
        }
    }
}
