using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using IBApi;
using IBTradingSystem.Broker.IB.messages;
using IBTradingSystem.Broker.IB;

namespace LifestyleCommon
{
    public class IBSite
    {
        private int m_nReqID = (new Random(DateTime.Now.Millisecond).Next() % 100) * 1000;
        private IBClient m_ibClient;
        private EReaderMonitorSignal signal = new EReaderMonitorSignal();
        private Dictionary<int, string> m_reqedSymbolDic = new Dictionary<int, string>();

        public void Init()
        {
            m_ibClient = new IBClient(signal);
            // m_ibClient.ConnectionClosed += IBClient_ConnectionClosed;

            // m_ibClient.Error += IBClient_Error; // get error message
            // m_ibClient.NextValidId += ConnectionEventCallBack;    // get connection event
            m_ibClient.tickByTickBidAsk += TickMsg; // get tick bid ask values.
                                                    // m_ibClient.OrderStatus += OnOrderUpdate;
        }

        public bool Connect(string sHost, int nPort, int nID)
        {
            try
            {
                m_ibClient.ClientId = nID;
                m_ibClient.ClientSocket.eConnect(sHost, nPort, nID);
                var reader = new EReader(m_ibClient.ClientSocket, signal);
                reader.Start();

                new Thread(() => { while (m_ibClient.ClientSocket.IsConnected()) { signal.waitForSignal(); reader.processMsgs(); } }) { IsBackground = true }.Start();
                return true;
            }
            catch (Exception e)
            {
                Global.OnLog("IB Connect Exception : " + e.Message);
                return false;
            }
        }

        public void SetHisUpdateAction(Action<string, string, double, double, double, double> onUpdate)
        {
            Action<HistoricalDataMessage> _onUpdate = (HistoricalDataMessage msg) =>
            {
                if (m_reqedSymbolDic.ContainsKey(msg.RequestId))
                {
                    onUpdate(m_reqedSymbolDic[msg.RequestId], msg.Date, msg.Open, msg.High, msg.Low, msg.Close);
                }
            };
            m_ibClient.HistoricalData += _onUpdate;
            //m_ibClient.HistoricalDataEnd += _onUpdate;
            m_ibClient.HistoricalDataUpdate += _onUpdate;
        }

        public void Disconnect()
        {
            m_ibClient.ClientSocket.eDisconnect();
        }

        public void Subscribe(List<Symbol> lstSymbol)
        {
            int id = 0;
            foreach (var symbol in lstSymbol)
            {
                m_ibClient.ClientSocket.reqTickByTickData(id, getContract(symbol), "BidAsk", 0, true);
            }
        }

        public void GetHistoricalData(Symbol symbol, DateTime dtStart)
        {
            int reqId = m_nReqID++;
            m_reqedSymbolDic[reqId] = symbol.m_sSymbol;
            
            m_ibClient.ClientSocket.reqHistoricalData(
                reqId, 
                getContract(symbol),
                dtStart.AddHours(2).ToString("yyyyMMdd HH:mm:ss") + " GMT", 
                "7200 S",
                "1 min", 
                "BID_ASK", 
                1, 
                1, 
                false, 
                null);
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

        void TickMsg(TickByTickBidAskMessage tick)
        {
            Console.WriteLine(string.Format("IB tick {0}, {1}, {2}", tick.Time, tick.BidPrice, tick.AskPrice));
        }

        private Contract getContract(Symbol symbol)
        {
            return new Contract()
            {
                SecType = symbol.m_sSecurity,
                Symbol = symbol.m_sSymbol.Substring(0, 3),
                Exchange = symbol.m_sExchange,
                Currency = symbol.m_sSymbol.Substring(3)
            };
        }
    }
}
