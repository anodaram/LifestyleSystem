﻿
namespace IBTradingSystem.Broker.IB.messages
{
    class TickReqParamsMessage
    {
        public int TickerId { get; private set; }
        public double MinTick { get; private set; }
        public string BboExchange { get; private set; }
        public int SnapshotPermissions { get; private set; }

        public TickReqParamsMessage(int tickerId, double minTick, string bboExchange, int snapshotPermissions)
        {
            TickerId = tickerId;
            MinTick = minTick;
            BboExchange = bboExchange;
            SnapshotPermissions = snapshotPermissions;
        }
    }
}
