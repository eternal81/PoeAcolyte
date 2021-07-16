using System;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using PoeAcolyte.Service;


namespace PoeAcolyte.DataTypes
{
    public class PoeLogEntry : IPoeLogEntry
    {
        #region Members

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
        public IPoeLogEntry.PoeLogEntryTypeEnum PoeLogEntryType { get; set; }

        #endregion

        /// <summary>
        /// Parses <paramref name="raw"/> to determine if client.txt entry is a
        /// whisper, system message or trade
        /// </summary>
        /// <param name="raw"></param>
        public PoeLogEntry(string raw)
        {
            Raw = raw;
            try
            {
                // Date and time split by first two spaces
                TimeStamp = DateTime.Parse(raw.Split(' ')[0] + " " + raw.Split(' ')[1], new DateTimeFormatInfo());
                
                SetLogEntryType();
                IsValid = true;
            }
            catch (Exception)
            {
                //Debug.Print("tossing invalid log " + e.Message);
                IsValid = false;
            }
        }

        #region Check and Populate
        
        /// <summary>
        /// Init helper function: checks whisper -> checks type of trade. defaults to system message
        /// </summary>
        private void SetLogEntryType()
        {
            if (CheckWhisper())
            {
                if (CheckPricedTrade()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.PricedTrade;
                else if (CheckBulkTrade()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.BulkTrade; // only check if not priced trade 
                else if (CheckUnpricedTrade())
                    PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.UnpricedTrade; // only check if not priced or bulk trade
                else PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.Whisper; // treat as general whisper if no trade information found
            }
            else // Some type of system message
            {
                if (CheckAreaLeft()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.AreaLeft;
                else if (CheckAreaJoin()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.AreaJoined;
                else if (CheckYouJoin()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.YouJoin;
                else PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.SystemMessage;
                Other = PoeRegex.SystemMessage.Match(Raw).Groups["Message"].Value;
            }
        }

        private bool CheckWhisper() //see if whisper and set flags (Outgoing/Incoming)
        {
            if (PoeRegex.WhisperFrom.IsMatch(Raw))
            {
                Player = PoeRegex.WhisperFrom.Match(Raw).Groups["From"].Value;
                Other = PoeRegex.WhisperFrom.Match(Raw).Groups["Other"].Value;
                Incoming = true;
            }
            else if (PoeRegex.WhisperTo.IsMatch(Raw))
            {
                Player = PoeRegex.WhisperTo.Match(Raw).Groups["To"].Value;
                Other = PoeRegex.WhisperTo.Match(Raw).Groups["Other"].Value;
                Outgoing = true;
            }
            
            if (!PoeRegex.Guild.IsMatch(Raw)) return (Incoming || Outgoing);
            
            // Override guild/player name if guild name is found
            Guild = PoeRegex.Guild.Match(Raw).Groups["Guild"].Value;
            Player = PoeRegex.Guild.Match(Raw).Groups["Player"].Value;

            return (Incoming || Outgoing);
        }

        private void CheckStashTab()
        {
            foreach (Regex regex in PoeRegex.StashTabList)
            {
                if (!regex.IsMatch(Raw)) continue;
                Top = int.Parse(regex.Match(Raw).Groups["Top"].Value);
                Left = int.Parse(regex.Match(Raw).Groups["Left"].Value);
                StashTab = regex.Match(Raw).Groups["StashTab"].Value;
                Other = regex.Match(Raw).Groups["Other"].Value;
                break;
            }
        }

        private bool CheckPricedTrade()
        {
            foreach (Regex regex in PoeRegex.PricedTradeList)
            {
                if (!regex.IsMatch(Raw)) continue;
                
                CheckStashTab();
                Item = regex.Match(Raw).Groups["Item"].Value;
                PriceAmount = int.Parse(regex.Match(Raw).Groups["PriceAmount"].Value);
                PriceUnits =  regex.Match(Raw).Groups["PriceUnit"].Value;
                League =  regex.Match(Raw).Groups["League"].Value;
                return true;
            }
            return false;
        }

        private bool CheckUnpricedTrade()
        {
            foreach (Regex regex in PoeRegex.UnpricedTradeList)
            {
                if (!regex.IsMatch(Raw)) continue;
                
                CheckStashTab();
                Item = regex.Match(Raw).Groups["Item"].Value;
                League =  regex.Match(Raw).Groups["League"].Value;
                return true;
            }
            return false;
        }

        private bool CheckBulkTrade()
        {
            foreach (Regex regex in PoeRegex.BulkTradeList)
            {
                if (!regex.IsMatch(Raw)) continue;
                PriceAmount = int.Parse(regex.Match(Raw).Groups["SellAmount"].Value);
                PriceUnits = regex.Match(Raw).Groups["SellUnits"].Value;
                BuyPriceAmount = int.Parse(regex.Match(Raw).Groups["BuyAmount"].Value);
                BuyPriceUnits = regex.Match(Raw).Groups["BuyUnits"].Value;
                League = regex.Match(Raw).Groups["League"].Value;
                Other = regex.Match(Raw).Groups["Other"].Value;
                return true;
            }
            return false;
        }

        private bool CheckAreaJoin()
        {
            foreach (Regex regex in PoeRegex.AreaJoinedList)
            {
                if (!regex.IsMatch(Raw)) continue;
                Player = regex.Match(Raw).Groups["Player"].Value;
                return true;
            }
            return false;
        }

        private bool CheckAreaLeft()
        {
            foreach (Regex regex in PoeRegex.AreaLeftList)
            {
                if (!regex.IsMatch(Raw)) continue;
                Player = regex.Match(Raw).Groups["Player"].Value;
                return true;
            }
            return false;
        }

        private bool CheckYouJoin()
        {
            foreach (Regex regex in PoeRegex.YouJoinList)
            {
                if (!regex.IsMatch(Raw)) continue;
                Area = regex.Match(Raw).Groups["Area"].Value;
                return true;
            }
            return false;
        }
        
        #endregion
    }
}