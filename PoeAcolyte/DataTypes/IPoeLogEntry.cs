using System;

namespace PoeAcolyte.DataTypes
{
    public interface IPoeLogEntry
    {
        public enum PoeLogEntryTypeEnum
        {
            Whisper,
            PricedTrade,
            UnpricedTrade,
            BulkTrade,
            AreaJoined,
            AreaLeft,
            YouJoin,
            SystemMessage
        }

        public string Other { get; set; }
        public string Area { get; set; }
        public string Item { get; set; }
        public int Top { get; set; }
        public int Left { get; set; }
        public string Guild { get; set; }
        public string Player { get; set; }
        public string StashTab { get; set; }
        public int PriceAmount { get; set; }
        public string PriceUnits { get; set; }
        public int BuyPriceAmount { get; set; }
        public string BuyPriceUnits { get; set; }
        public string League { get; set; }
        public bool Incoming { get; set; }
        public bool Outgoing { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Raw { get; set; }
        public bool IsValid { get; set; }
        public PoeLogEntryTypeEnum PoeLogEntryType { get; set; }
    }
}