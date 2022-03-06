using System;
using System.Text;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadSerialNumberRequest : Request
    {
        #region Constructor

        protected ReadSerialNumberRequest(string commandCode) : base(commandCode)
        {
        }

        #endregion


        #region Public Methods

#if NETSTANDARD
        public ReadSerialNumberResponse UnpackResponseMessage(byte[] responseMessage)
        {
            return ReadSerialNumberResponse.UnpackResponseMessage(this, responseMessage);
        }
#else
        public ReadSerialNumberResponse UnpackResponseMessage(Memory<byte> responseMessage)
        {
            return ReadSerialNumberResponse.UnpackResponseMessage(this, responseMessage);
        }
#endif

        public static ReadSerialNumberRequest CreateNew(MettlerToledoDevice device)
        {
            return new ReadSerialNumberRequest(Commands.ReadSerialNumber);
        }

        #endregion


        #region Protected Methods

        protected override void BuildMessageDetail(ref StringBuilder messageBuilder)
        {
        }

        #endregion
    }
}
