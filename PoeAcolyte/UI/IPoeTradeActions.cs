using System;
using PoeAcolyte.DataTypes;

namespace PoeAcolyte.UI
{
    public interface IPoeTradeActions
    {
        public Action Invite { get; }
  
        public Action<string> Reset { get; }

        public Action AskToWait { get; }

        public Action Trade { get; }

        public Action Decline { get; }

        public Action ThanksGoodbye { get; }

        public Action WhoIs { get; }

        public Action Close { get; }
    }
}