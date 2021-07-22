using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Gma.System.MouseKeyHook;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;

namespace PoeAcolyte.Helpers
{
    /// <summary>
    /// Used to broker events from the client log, interact with the POE client, manage trade requests/controls
    /// and load/save/modify settings. Also responsible for managing cross thread calls to GUI
    /// </summary>
    public class PoeGameBroker
    {
        /// <summary>
        /// Default constructor. Auto loads in <see cref="PoeSettings"/> - You must specify <see cref="PoeGameService"/>,
        /// <see cref="PoeLogReader"/> to hook to log and client events
        /// </summary>
        public PoeGameBroker()
        {
            Settings = PoeSettings.Load();
            Hook.GlobalEvents().MouseClick += OnMouseClick;
            // Service = new PoeGameService();
            // LogReader = new PoeLogReader();
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.YouJoin"/>
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnYouJoin(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // TODO add area search pattern with average time between map or specific zones
            // (i.e. avg time to kill shaper) minus time currently spent in zone
            foreach (var tradeControl in ActiveTrades)
            {
                tradeControl.IsBusy = !e.LogEntry.Area.Contains("Hideout");
            }
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.Whisper"/> checks each active trade if the player that whispered
        /// is attached to
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnWhisper(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            foreach (var trade in ActiveTrades.Where(trades => trades.Players.Contains(e.LogEntry.Player)))
            {
                // Thread safe to UI
                trade.GetUserControl?.Invoke(new Action(() => { trade.TakeLogEntry(e.LogEntry); }));
            }
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.BulkTrade"/>
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnBulkTrade(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Add to existing if duplicate
            if (DuplicateItem(e.LogEntry)) return;

            // Brand new request
            AddTrade( new BulkTrade(e.LogEntry));
        }

        /// <summary>
        /// Add the trade control (Has to be cross thread safe to UI)
        /// </summary>
        /// <param name="tradeControl"></param>
        private void AddTrade(ITrade tradeControl)
        {
            
            tradeControl.Disposed += (_, _) =>
            {
                // TODO add a memory of trades that were out of stock/sold etc to ignore for a given timeframe
                // OutOfStock, TradeComplete, Declined
                TradePanel.Controls.Remove(tradeControl.GetUserControl);
                ActiveTrades.Remove(tradeControl);
            };
            ActiveTrades.Add(tradeControl);
            
            // Needs to be thread safe to UI
            TradePanel.Invoke(new Action(() => { TradePanel.Controls.Add(tradeControl.GetUserControl); }));
        }

        /// <summary>
        /// Event handler for <see cref="IPoeLogReader.PricedTrade"/>, <see cref="IPoeLogReader.BulkTrade"/>,
        /// <see cref="IPoeLogReader.UnpricedTrade"/>
        /// </summary>
        /// <param name="sender">not useful / null</param>
        /// <param name="e"><see cref="IPoeLogReader.PoeLogEventArgs"/></param>
        private void LogReaderOnSingleTrade(object sender, IPoeLogReader.PoeLogEventArgs e)
        {
            // Add to existing if duplicate
            if (DuplicateItem(e.LogEntry)) return;

            AddTrade(new SingleTrade(e.LogEntry));
        }

        /// <summary>
        /// See if item already has an existing trade control (Has to be cross threaded to UI)
        /// </summary>
        /// <param name="e"><see cref="PoeLogEntry"/> to compare against</param>
        /// <returns>true if <see cref="ActiveTrades"/> contains the entry, false if not</returns>
        private bool DuplicateItem(PoeLogEntry e)
        {
            var bTaken = false;
            foreach (var trade in ActiveTrades.Where(trade =>
                trade.ActiveLogEntry.PoeLogEntryType == e.PoeLogEntryType))
            {
                // Cross thread to UI
                trade.GetUserControl.Invoke(new Action(() =>
                {
                    if (trade.TakeLogEntry(e)) bTaken = true;
                }));
            }

            return bTaken;
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
        /// For debugging and testing, fires all
        /// </summary>
        public void ManualFire()
        {
            using StreamReader stream = new StreamReader(File.Open(
                @"C:\Program Files (x86)\Grinding Gear Games\Path of Exile\logs\test.txt",
                FileMode.Open, FileAccess.Read, FileShare.ReadWrite));

            while (!stream.EndOfStream)
            {
                LogReader.ManualFire(stream.ReadLine());
            }
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
        public Control TradePanel { get; set; }

        /// <summary>
        /// Broker to handle interaction with the POE client
        /// </summary>
        public PoeGameService Service
        {
            get => _service;
            set
            {
                _service = value;
                // Automatically search client log if POE is open
                Service.PoeConnected += (_, args) =>
                {
                    Program.Log.Verbose("PoeGameBroker - Client {conn}",
                        args.IsConnected ? "connected" : "disconnected");
                    LogReader.IsRunning = args.IsConnected;
                };
            }
        }

        private void OnMouseClick(object sender, MouseEventArgs e)
        {
            foreach (ITrade trade in ActiveTrades)
            {
                trade.TakeMouseClick(e);
            }
        }

        /// <summary>
        /// Broker to handle interaction with the POE logs
        /// </summary>
        public PoeLogReader LogReader
        {
            get => _logReader;
            set
            {
                _logReader = value;
                LogReader.PricedTrade += LogReaderOnSingleTrade;
                LogReader.BulkTrade += LogReaderOnBulkTrade;
                LogReader.Whisper += LogReaderOnWhisper;
                LogReader.YouJoin += LogReaderOnYouJoin;
                LogReader.UnpricedTrade += LogReaderOnSingleTrade;
            }
        }

        /// <summary>
        /// List of active trades (used for routing log entries)
        /// </summary>
        public List<ITrade> ActiveTrades { get; } = new();

        #endregion

        private PoeGameService _service;
        private PoeLogReader _logReader;
    }
}