namespace PoeAcolyte.Service
{
    /// <summary>
    /// Interface for sending whispers or other types of commands to the POE client
    /// </summary>
    public interface IPoeCommands
    {
        /// <summary>
        /// Type of command to send
        /// </summary>
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

        /// <summary>
        /// Send command based on a player
        /// </summary>
        /// <param name="type"><see cref="CommandType"/></param>
        /// <param name="player">Player Name</param>
        public void SendPoeCommand(CommandType type, string player);
        /// <summary>
        /// Send a command
        /// </summary>
        /// <param name="type"><see cref="CommandType"/></param>
        public void SendPoeCommand(CommandType type);
        /// <summary>
        /// Send a whisper to a player
        /// </summary>
        /// <param name="player"></param>
        /// <param name="message"></param>
        public void SendPoeWhisper(string player, string message);
    }
}