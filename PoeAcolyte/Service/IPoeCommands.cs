namespace PoeAcolyte.Service
{
    public interface IPoeCommands
    {
        public enum CommandType
        {
            /// <summary>
            /// /invite "character"
            /// </summary>
            Invite,

            /// <summary>
            /// /tradewith "character" 
            /// </summary>
            Trade,

            /// <summary>
            /// /hideout or /hideout "character"
            /// </summary>
            Hideout,

            /// <summary>
            /// /kick 'character"
            /// </summary>
            Kick,

            /// <summary>
            /// /delve 
            /// </summary>
            Delve,

            /// <summary>
            /// /menagerie
            /// </summary>
            Menagerie,

            /// <summary>
            /// /metamorph
            /// </summary>
            Metamorph
        }

        public void SendPoeCommand(CommandType type, string player);
        public void SendPoeCommand(CommandType type);
        public void SendPoeWhisper(string player, string message);
    }
}