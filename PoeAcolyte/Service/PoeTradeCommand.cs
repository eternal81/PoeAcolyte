using System;

namespace PoeAcolyte.Service
{
    public class PoeTradeCommand
    {
        public Action<string> Invite => playerName =>
        {
            Program.GameBroker.Service.SendCommandToClient($"/invite {playerName}");
        };
    }
}