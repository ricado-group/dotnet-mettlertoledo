using System;

namespace RICADO.MettlerToledo
{
    public class CommandResult : RequestResult
    {
        #region Constructor

        internal CommandResult(Channels.ProcessMessageResult result) : base(result)
        {
        }

        #endregion
    }
}
