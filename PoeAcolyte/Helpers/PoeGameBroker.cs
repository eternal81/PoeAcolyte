using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;
using PoeAcolyte.UI;

namespace PoeAcolyte.Helpers
{
    public class PoeGameBroker
    {
        public PoeGameService Service { get; set; }
        public PoeLogReader LogReader { get; set; }
        public PoeSettings Settings { get; set; }
        public Control TradePanel { get; set; }
        public List<IPoeTradeControl> TradeControls { get; set; } = new();
        
        public PoeGameBroker()
        {
            Settings = PoeSettings.Load();
            Service = new PoeGameService();
            LogReader = new PoeLogReader();
            LogReader.PricedTrade += LogReaderOnTrade;
            LogReader.BulkTrade += LogReaderOnTrade;
            LogReader.Whisper += LogReaderOnWhisper;
            LogReader.YouJoin += LogReaderOnYouJoin;
            LogReader.UnpricedTrade += LogReaderOnTrade;
        }


        private void LogReaderOnYouJoin(object? sender, IPoeLogReader.PoeLogEventArgs e)
        {
            foreach (var tradeControl in TradeControls)
            {
                tradeControl.IsBusy = !e.LogEntry.Area.Contains("Hideout");
            }
            
        }

        private void LogReaderOnWhisper(object? sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Only handle incoming whispers
            if (!e.LogEntry.Incoming) return;
            foreach (var broker in TradeControls.Where(broker => broker.Players.Contains(e.LogEntry.Player)))
            {
                broker.AddWhisper(e.LogEntry);
            }
        }

        public void LogReaderOnTrade(object? sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Only handle incoming trade request
            if (!e.LogEntry.Incoming) return;
            
            // Add to existing if duplicate
            if (DuplicateItem(e.LogEntry)) return;

            // Brand new request
            IPoeTradeControl tradeControl = new PoeTradeControl(e.LogEntry, Service);
       
            tradeControl.Disposed += (o, args) =>
            {
                TradePanel.Controls.Remove(tradeControl.GetUserControl);
            };
            TradeControls.Add(tradeControl);
            TradePanel.Controls.Add(tradeControl.GetUserControl);
        }

        private bool DuplicateItem(PoeLogEntry e)
        {
            var bFound = false;

            // TODO does this need to include the non active entries?
            var duplicates = TradeControls.Where(incoming =>
                incoming.ActiveLogEntry.Item == e.Item &&
                incoming.ActiveLogEntry.Top == e.Top &&
                incoming.ActiveLogEntry.Left == e.Left);

            // Duplicate trade (possible extra message at end or other players)
            foreach (var broker in duplicates)
            {
                broker.AddPlayer(e);
                bFound = true;
            }

            return bFound;
        }

        /// <summary>
        /// For debugging and testing
        /// </summary>
        /// <param name="raw"></param>
        public void ManualFire(string raw)
        {
            LogReader.ManualFire(raw);
        }
        
        public void RefreshUI()
        {
            if (TradePanel is not null)
            {
                TradePanel.Location = Settings.Trades.Location;
                TradePanel.Size = Settings.Trades.Size;
            }
        }
    }
}