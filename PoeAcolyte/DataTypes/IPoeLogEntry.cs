using System;

namespace PoeAcolyte.DataTypes
{
    /// <summary>
    /// Interface for log entries
    /// </summary>
    public interface IPoeLogEntry
    {
        /// <summary>
        /// What type of log entry is detected in the client.txt
        /// <para>Based on the <see cref="Service.PoeLogReader"/> event</para>
        /// </summary>
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

        /// <summary>
        /// Anything found after a standard trade message or any non trade message with @From:
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.UnpricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// <seealso cref="Service.PoeRegex.WhisperTo"/>
        /// <seealso cref="Service.PoeRegex.WhisperFrom"/>
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// Player enters your area <br></br>
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.AreaJoinedList"/>
        /// </summary>
        public string Area { get; set; }
        /// <summary>
        /// Item being traded for
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.UnpricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// Top (Y) location of stash tab (1,1) being (left, top) most item
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// </summary>
        public int Top { get; set; }
        /// <summary>
        /// Left (X) location of stash tab (1,1) being (left, top) most item
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// </summary>
        public int Left { get; set; }
        /// <summary>
        /// [Optional] Guild name if specified
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.Guild"/>
        /// </summary>
        public string Guild { get; set; }
        /// <summary>
        /// Player Name
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.WhisperFrom"/>
        /// <seealso cref="Service.PoeRegex.WhisperTo"/>
        /// <seealso cref="Service.PoeRegex.Guild"/>
        /// <seealso cref="Service.PoeRegex.AreaJoinedList"/>
        /// <seealso cref="Service.PoeRegex.AreaLeftList"/>
        /// </summary>
        public string Player { get; set; }
        /// <summary>
        /// Name of stash tab item is located
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.StashTabList"/>
        /// </summary>
        public string StashTab { get; set; }
        /// <summary>
        /// Number of <see cref="PriceUnits"/> that player is paying
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.UnpricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// </summary>
        public int PriceAmount { get; set; }
        /// <summary>
        /// Unit of currency <see cref="PriceUnits"/>player is paying
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.UnpricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// </summary>
        public string PriceUnits { get; set; }
        /// <summary>
        /// Bulk trade - Number of <see cref="BuyPriceUnits"/> that player want
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// </summary>
        public int BuyPriceAmount { get; set; }
        /// <summary>
        /// Bulk trade - Unit of currency <see cref="BuyPriceAmount"/> that player wants
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// </summary>
        public string BuyPriceUnits { get; set; }
        /// <summary>
        /// League trade is requested on
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.PricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.UnpricedTradeList"/>
        /// <seealso cref="Service.PoeRegex.BulkTradeList"/>
        /// </summary>
        public string League { get; set; }
        /// <summary>
        /// Incoming message (@From)
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.WhisperFrom"/>
        /// </summary>
        public bool Incoming { get; set; }
        /// <summary>
        /// Outgoing message (@To)
        /// <seealso cref="Service.PoeRegex"/>
        /// <seealso cref="Service.PoeRegex.WhisperTo"/>
        /// </summary>
        public bool Outgoing { get; set; }
        /// <summary>
        /// Date/Time of log entry as recorded in the client file (Parsed in the constructor of
        /// <see cref="PoeLogEntry"/>)
        /// </summary>
        public DateTime TimeStamp { get; set; }
        /// <summary>
        /// Raw string from log entry
        /// </summary>
        public string Raw { get; set; }
        /// <summary>
        /// Was log entry able to be parsed?
        /// </summary>
        public bool IsValid { get; set; }
        /// <summary>
        /// What type of log entry it is
        /// </summary>
        public PoeLogEntryTypeEnum PoeLogEntryType { get; set; }
        
    }
}