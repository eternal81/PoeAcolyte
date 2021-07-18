﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using PoeAcolyte.DataTypes;
using PoeAcolyte.Service;

namespace PoeAcolyte.UI
{
    public class Trade : IDisposable
    {        
        /// <summary>
        /// /////////////////////////////////// get rid of
        /// </summary>
        public IPoeCommands Commands { get; set; }
        
        protected ToolStripMenuItem PlayersMenu = new("Players");
        protected PoeLogEntry ActiveEntry;
        protected readonly List<PoeLogEntry> LogEntries = new();
        public bool IsBusy { get; set; }
        public IPoeTradeControl.TradeStatus ActiveTradeStatus { get; set; }
        public IEnumerable<string> Players => LogEntries.Select(o => o.Player).Distinct();
        public virtual PoeLogEntry ActiveLogEntry { get=>ActiveEntry; set=>ActiveEntry=value; }
        public event EventHandler Disposed;
        public virtual UserControl GetUserControl { get; set; }
        
        public Trade(PoeLogEntry entry)
        {
            LogEntries.Add(entry);

            PlayersMenu.DropDownItems.Add(MakeMenuItem(entry, logEntry => { ActiveEntry = logEntry;}));
        }

        public virtual bool TakeLogEntry(PoeLogEntry entry)
        {
            return false;
        }
        
        public void RemoveTrade(PoeLogEntry entry, bool bRemoveAll = false)
        {
            LogEntries.Remove(entry);
            if (LogEntries.Any() && !bRemoveAll)
                ActiveEntry = LogEntries[0];
            else
                Dispose();
        }

        public bool CheckWhisper(PoeLogEntry entry)
        {
            if (entry.PoeLogEntryType != IPoeLogEntry.PoeLogEntryTypeEnum.Whisper ||
                !Players.Contains(entry.Player)) return false;
            
            var match =  PlayersMenu.DropDownItems.Find(entry.Player, false);
            if (!match.Any()) return false;
            
            var menuItem = (ToolStripMenuItem)match.First();
            menuItem.DropDownItems.Add(entry.Other);
            LogEntries.Add(entry);
            return true;
        }

        public Action Invite => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/invite {ActiveLogEntry.Player}");
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} Ready for pickup");
            ActiveTradeStatus = IPoeTradeControl.TradeStatus.Invited;
        };

        public Action NoStock => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} Sold out");
            RemoveTrade(ActiveEntry);
        };

        public Action Decline => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"@{ActiveLogEntry.Player} No thanks");
            Program.GameBroker.Service.SendCommandToClient($"/kick {ActiveLogEntry.Player}");
            RemoveTrade(ActiveEntry);
        };
        public Action DoTrade => () =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/tradewith {ActiveLogEntry.Player}");
            RemoveTrade(ActiveEntry);
        };
        public Action Close => Dispose;


        public virtual void Dispose()
        {
            
        }
        /// <summary>
        /// Generic context menu add
        /// </summary>
        /// <param name="text"></param>
        /// <param name="action"></param>
        /// <param name="checkOnClick"></param>
        /// <returns></returns>
        public static ToolStripMenuItem MakeMenuItem(string text, Action action, bool checkOnClick = false)
        {
            var ret = new ToolStripMenuItem(text){Name = text};
            ret.Click += (sender, args) =>
            {
                ret.Checked = checkOnClick;
                action();
            };
            return ret;
        }
        /// <summary>
        /// Used to make player entries for context menu
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="action"></param>
        /// <param name="checkOnClick"></param>
        /// <returns></returns>
        public static ToolStripMenuItem MakeMenuItem(PoeLogEntry entry, Action<PoeLogEntry> action, bool checkOnClick = false)
        {
            var text = $"{entry.Player}  {entry.PriceAmount} {entry.PriceUnits} ► {entry.BuyPriceAmount} {entry.BuyPriceUnits}";
            var ret = new ToolStripMenuItem(text){Name = entry.Player};
            ret.Click += (sender, args) =>
            {
                ret.Checked = checkOnClick;
                action(entry);
            };
            return ret;
        }
    }
}