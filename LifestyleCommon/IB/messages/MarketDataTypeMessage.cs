﻿/* IBTradingSystem. IB Client.
 * Get all data from IB gateway.. */

namespace IBTradingSystem.Broker.IB.messages
{
    class MarketDataTypeMessage
    {
        protected int requestId;
        protected int marketDataType;

        public MarketDataTypeMessage(int requestId, int marketDataType)
        {
            RequestId = requestId;
            MarketDataType = marketDataType;
        }

        public int RequestId
        {
            get { return requestId; }
            set { requestId = value; }
        }

        public int MarketDataType
        {
            get { return marketDataType; }
            set { marketDataType = value; }
        }
    }
}
