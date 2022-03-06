using System;
using System.Text;

namespace RICADO.MettlerToledo.SICS
{
    internal class CommandRequest : Request
    {
        #region Private Fields

        private readonly CommandType _command;

        #endregion


        #region Public Properties

        public CommandType Command => _command;

        #endregion


        #region Constructor

        protected CommandRequest(CommandType command, string commandCode) : base(commandCode)
        {
            _command = command;
        }

        #endregion


        #region Public Methods

#if NETSTANDARD
        public void ValidateResponseMessage(byte[] responseMessage)
#else
        public void ValidateResponseMessage(Memory<byte> responseMessage)
#endif
        {
            CommandResponse.ValidateResponseMessage(this, responseMessage);
        }

        public static CommandRequest CreateNew(MettlerToledoDevice device, CommandType command)
        {
            switch(command)
            {
                case CommandType.ZeroStable:
                    return new CommandRequest(CommandType.ZeroStable, Commands.ZeroStableCommand);

                case CommandType.ZeroImmediately:
                    return new CommandRequest(CommandType.ZeroImmediately, Commands.ZeroImmediatelyCommand);

                case CommandType.TareStable:
                    return new CommandRequest(CommandType.TareStable, Commands.TareStableCommand);

                case CommandType.TareImmediately:
                    return new CommandRequest(CommandType.TareImmediately, Commands.TareImmediatelyCommand);

                case CommandType.ClearTare:
                    return new CommandRequest(CommandType.ClearTare, Commands.ClearTareCommand);
            }

            throw new SICSException("Unknown Command Type '" + command.ToString() + "'");
        }

        #endregion


        #region Protected Methods

        protected override void BuildMessageDetail(ref StringBuilder messageBuilder)
        {
        }

        #endregion
    }
}
