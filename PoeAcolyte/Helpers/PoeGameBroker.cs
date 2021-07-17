using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;
using PoeAcolyte.UI;

namespace PoeAcolyte.Helpers
{
    /// <summary>
    /// Used to broker events from the client log, interact with the POE client, manage trade requests/controls
    /// and load/save/modify settings
    /// </summary>
    public class PoeGameBroker
    {
        /// <summary>
        /// Default constructor. Loads in <see cref="PoeSettings"/>, <see cref="PoeGameService"/>,
        /// <see cref="PoeLogReader"/> and hooks to log events
        /// </summary>
        private PoeGameBroker()
        {
            Settings = PoeSettings.Load();
            Service = new PoeGameService();
            LogReader = new PoeLogReader();
            LogReader.PricedTrade += LogReaderOnTrade;
            LogReader.BulkTrade += LogReaderOnBulkTrade;
            LogReader.Whisper += LogReaderOnWhisper;
            LogReader.YouJoin += LogReaderOnYouJoin;
            LogReader.UnpricedTrade += LogReaderOnTrade;
            
            // Automatically search client log if POE is open
            Service.PoeConnected += (_, args) =>
            {
                Program.Log.Verbose("PoeGameBroker - Client {conn}", args.IsConnected ? "connected":"disconnected");
                LogReader.IsRunning = args.IsConnected;
            };
        }

        /// <summary>
        /// Constructor that sets up <see cref="TradePanel"/>
        /// </summary>
        /// <param name="tradePanel"><see cref="TradePanel"/> to use</param>
        public PoeGameBroker(Control tradePanel) : this()
        {
            TradePanel = tradePanel;
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.YouJoin"/>
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnYouJoin(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            foreach (var tradeControl in TradeControls)
            {
                tradeControl.IsBusy = !e.LogEntry.Area.Contains("Hideout");
            }
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.Whisper"/>
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnWhisper(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Only handle incoming whispers
            if (!e.LogEntry.Incoming) return;
            foreach (var broker in TradeControls.Where(broker => broker.Players.Contains(e.LogEntry.Player)))
            {
                broker.AddLogEntry(e.LogEntry);
            }
        }

        private void LogReaderOnBulkTrade(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Only handle incoming trade request
            if (!e.LogEntry.Incoming) return;

            // Add to existing if duplicate
            if (DuplicateItem(e.LogEntry)) return;
            
            // Brand new request
            IPoeTradeControl tradeControl = new PoeTradeControl(e.LogEntry, Service);

            tradeControl.Disposed += (_, _) =>
            {
                TradePanel.Controls.Remove(tradeControl.GetUserControl);
                TradeControls.Remove(tradeControl);
            };
            TradeControls.Add(tradeControl);
            TradePanel.Controls.Add(tradeControl.GetUserControl);
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.PricedTrade"/>, <see cref="IPoeLogReader.BulkTrade"/>,
        /// <see cref="IPoeLogReader.UnpricedTrade"/>
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnTrade(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Only handle incoming trade request
            if (!e.LogEntry.Incoming) return;

            // Add to existing if duplicate
            if (DuplicateItem(e.LogEntry)) return;

            // Brand new request
            IPoeTradeControl tradeControl = new PoeTradeControl(e.LogEntry, Service);

            tradeControl.Disposed += (_, _) =>
            {
                TradePanel.Controls.Remove(tradeControl.GetUserControl);
                TradeControls.Remove(tradeControl);
            };
            TradeControls.Add(tradeControl);
            TradePanel.Controls.Add(tradeControl.GetUserControl);
        }

        /// <summary>
        /// See if item already has an existing trade control
        /// </summary>
        /// <param name="e"><see cref="PoeLogEntry"/> to compare against</param>
        /// <returns>true if <see cref="TradeControls"/> contains the entry, false if not</returns>
        private bool DuplicateItem(PoeLogEntry e)
        {
            var bFound = false;

            var duplicates = TradeControls.Where(incoming =>
                incoming.ActiveLogEntry.Item == e.Item &&
                incoming.ActiveLogEntry.Top == e.Top &&
                incoming.ActiveLogEntry.Left == e.Left);

            // Duplicate trade (possible extra message at end or other players)
            foreach (var broker in duplicates)
            {
                broker.AddLogEntry(e);
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

        /// <summary>
        /// Update control locations based on the settings (<see cref="TradePanel"/>)
        /// </summary>
        public void RefreshUI()
        {
            if (TradePanel is null) return;
            TradePanel.Location = Settings.Trades.Location;
            TradePanel.Size = Settings.Trades.Size;
        }

        #region Members

        /// <summary>
        /// Broker used to save/load/modify the session to session settings
        /// </summary>
        public PoeSettings Settings { get; }

        /// <summary>
        /// Control container (FlowLayoutPanel) to populate with trade requests
        /// </summary>
        private Control TradePanel { get; set; }

        /// <summary>
        /// Broker to handle interaction with the POE client
        /// </summary>
        private PoeGameService Service { get; }

        /// <summary>
        /// Broker to handle interaction with the POE logs
        /// </summary>
        private PoeLogReader LogReader { get; }

        /// <summary>
        /// List of active trades (used for routing log entries)
        /// </summary>
        private List<IPoeTradeControl> TradeControls { get; } = new();

        #endregion
    }
}