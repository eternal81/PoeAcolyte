using System;
using System.Globalization;
using System.Text.RegularExpressions;
using PoeAcolyte.Service;


namespace PoeAcolyte.DataTypes
{
    /// <summary>
    /// Parses client.txt log entry to determine what type <see cref="IPoeLogEntry.PoeLogEntryTypeEnum"/>
    /// (whisper, system message, trade etc) and fills out entry based on <see cref="PoeRegex"/>
    /// </summary>
    public class PoeLogEntry : IPoeLogEntry
    {
        #region IPoeLogEntry

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
                // Date and time split by first two spaces "2021/07/04 08:38:45 67873453 bad [INFO Client 22384] @From Fu..."
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

        /// <summary>
        /// Init helper function: checks whisper -> checks type of trade. defaults to system message
        /// </summary>
        private void SetLogEntryType()
        {
            if (CheckWhisper())
            {
                if (CheckPricedTrade()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.PricedTrade;
                // only check if bulk if not priced trade
                else if (CheckBulkTrade()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.BulkTrade;
                // only check if unpriced if not priced or bulk trade
                else if (CheckUnpricedTrade()) PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.UnpricedTrade;
                // treat as general whisper if no trade information found
                else PoeLogEntryType = IPoeLogEntry.PoeLogEntryTypeEnum.Whisper;
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

        #region Check and Populate

        /// <summary>
        /// Populates values if <see cref="PoeRegex.WhisperFrom"/> or <see cref="PoeRegex.WhisperTo"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Player"/>, <see cref="Other"/>, (<see cref="Incoming"/>/<see cref="Outgoing"/>)</value>
        private bool CheckWhisper() //see if whisper and set flags (Outgoing/Incoming)
        {
            if (PoeRegex.WhisperFrom.IsMatch(Raw))
            {
                Player = PoeRegex.WhisperFrom.Match(Raw).Groups["Player"].Value;
                Other = PoeRegex.WhisperFrom.Match(Raw).Groups["Other"].Value;
                Incoming = true;
            }
            else if (PoeRegex.WhisperTo.IsMatch(Raw))
            {
                Player = PoeRegex.WhisperTo.Match(Raw).Groups["Player"].Value;
                Other = PoeRegex.WhisperTo.Match(Raw).Groups["Other"].Value;
                Outgoing = true;
            }

            if (!PoeRegex.Guild.IsMatch(Raw)) return (Incoming || Outgoing);

            // Override guild/player name if guild name is found
            Guild = PoeRegex.Guild.Match(Raw).Groups["Guild"].Value;
            Player = PoeRegex.Guild.Match(Raw).Groups["Player"].Value;

            return (Incoming || Outgoing);
        }

        /// <summary>
        /// Populates values if <see cref="PoeRegex.StashTabList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Top"/>, <see cref="Left"/>, <see cref="StashTab"/>, <see cref="Other"/></value>
        private bool CheckStashTab()
        {
            foreach (Regex regex in PoeRegex.StashTabList)
            {
                if (!regex.IsMatch(Raw)) continue;
                Top = int.Parse(regex.Match(Raw).Groups["Top"].Value);
                Left = int.Parse(regex.Match(Raw).Groups["Left"].Value);
                StashTab = regex.Match(Raw).Groups["StashTab"].Value;
                Other = regex.Match(Raw).Groups["Other"].Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Populates values if <see cref="PoeRegex.PricedTradeList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Item"/>, <see cref="PriceAmount"/>, <see cref="PriceUnits"/>, <see cref="League"/></value>
        private bool CheckPricedTrade()
        {
            foreach (Regex regex in PoeRegex.PricedTradeList)
            {
                if (!regex.IsMatch(Raw)) continue;

                Item = regex.Match(Raw).Groups["Item"].Value;
                PriceAmount = int.Parse(regex.Match(Raw).Groups["PriceAmount"].Value);
                PriceUnits = regex.Match(Raw).Groups["PriceUnit"].Value;
                League = regex.Match(Raw).Groups["League"].Value;

                if (!CheckStashTab()) continue;

                return true;
            }

            return false;
        }

        /// <summary>
        /// Populates values if <see cref="PoeRegex.UnpricedTradeList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Item"/>, <see cref="League"/></value>
        private bool CheckUnpricedTrade()
        {
            foreach (Regex regex in PoeRegex.UnpricedTradeList)
            {
                if (!regex.IsMatch(Raw)) continue;

                CheckStashTab();
                Item = regex.Match(Raw).Groups["Item"].Value;
                League = regex.Match(Raw).Groups["League"].Value;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Populates values if <see cref="PoeRegex.BulkTradeList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="PriceAmount"/>, <see cref="PriceUnits"/>, <see cref="BuyPriceAmount"/>, 
        /// <see cref="BuyPriceUnits"/>, <see cref="League"/>, <see cref="Other"/></value>
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

        /// <summary>
        /// Populates values if <see cref="PoeRegex.AreaJoinedList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Player"/></value>
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

        /// <summary>
        /// Populates values if <see cref="PoeRegex.AreaLeftList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Player"/></value>
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

        /// <summary>
        /// Populates values if <see cref="PoeRegex.YouJoinList"/> is matched
        /// </summary>
        /// <returns>true if matched false otherwise</returns>
        /// <value><see cref="Area"/></value>
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

        /// <summary>
        /// Reformat class field into easier to read format
        /// </summary>
        /// <returns>description with appropriate fields</returns>
        public override string ToString()
        {
            return PoeLogEntryType switch
            {
                IPoeLogEntry.PoeLogEntryTypeEnum.BulkTrade =>
                    $"{BuyPriceAmount} {BuyPriceUnits}\r\n" +
                    $"{Player} ({PoeLogEntryType})\r\n" + $"{League}",
                IPoeLogEntry.PoeLogEntryTypeEnum.PricedTrade =>
                    $"{Item}\r\n" + $"({StashTab})\r\n" +
                    $"Top: {Top}, Left: {Left}\r\n" +
                    $"{Player} ({PoeLogEntryType})" + $"{League}",
                IPoeLogEntry.PoeLogEntryTypeEnum.UnpricedTrade =>
                    $"{Item}\r\n" + $"{Player} ({PoeLogEntryType} )\r\n" +
                    $"{League}",
                IPoeLogEntry.PoeLogEntryTypeEnum.Whisper => $"({Player} - {Other}",
                IPoeLogEntry.PoeLogEntryTypeEnum.AreaJoined => $"{Player} joined",
                IPoeLogEntry.PoeLogEntryTypeEnum.AreaLeft => $"{Player} left",
                IPoeLogEntry.PoeLogEntryTypeEnum.YouJoin => $"You entered {Area}",
                IPoeLogEntry.PoeLogEntryTypeEnum.SystemMessage => $"System Message - {Other}",
                _ => ""
            };
        }
    }
}