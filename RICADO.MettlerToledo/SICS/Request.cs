using System;
using System.Text;

namespace RICADO.MettlerToledo.SICS
{
    internal abstract class Request
    {
        #region Constants

        public const string ETX = "\r\n";

        #endregion


        #region Private Fields

        private readonly string _commandCode;

        #endregion


        #region Public Properties

        public string CommandCode => _commandCode;

        #endregion


        #region Constructor

        protected Request(string commandCode)
        {
            _commandCode = commandCode;
        }

        #endregion


        #region Public Methods

#if NETSTANDARD
        public byte[] BuildMessage()
#else
        public ReadOnlyMemory<byte> BuildMessage()
#endif
        {
            StringBuilder messageBuilder = new StringBuilder();

            messageBuilder.Append(_commandCode);

            BuildMessageDetail(ref messageBuilder);

            messageBuilder.Append(ETX);

            return Encoding.ASCII.GetBytes(messageBuilder.ToString());
        }

        #endregion


        #region Protected Methods

        protected abstract void BuildMessageDetail(ref StringBuilder messageBuilder);

        #endregion
    }
}
