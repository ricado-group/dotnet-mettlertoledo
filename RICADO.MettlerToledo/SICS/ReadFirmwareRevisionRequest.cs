using System;
using System.Text;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadFirmwareRevisionRequest : Request
    {
        #region Constructor

        protected ReadFirmwareRevisionRequest(string commandCode) : base(commandCode)
        {
        }

        #endregion


        #region Public Methods

#if NETSTANDARD
        public ReadFirmwareRevisionResponse UnpackResponseMessage(byte[] responseMessage)
        {
            return ReadFirmwareRevisionResponse.UnpackResponseMessage(this, responseMessage);
        }
#else
        public ReadFirmwareRevisionResponse UnpackResponseMessage(Memory<byte> responseMessage)
        {
            return ReadFirmwareRevisionResponse.UnpackResponseMessage(this, responseMessage);
        }
#endif

        public static ReadFirmwareRevisionRequest CreateNew(MettlerToledoDevice device)
        {
            return new ReadFirmwareRevisionRequest(Commands.ReadFirmwareRevision);
        }

        #endregion


        #region Protected Methods

        protected override void BuildMessageDetail(ref StringBuilder messageBuilder)
        {
        }

        #endregion
    }
}
