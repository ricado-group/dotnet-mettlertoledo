using System;
using System.Text;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadTareWeightRequest : Request
    {
        #region Constructor

        protected ReadTareWeightRequest(string commandCode) : base(commandCode)
        {
        }

        #endregion


        #region Public Methods

#if NETSTANDARD
        public ReadTareWeightResponse UnpackResponseMessage(byte[] responseMessage)
        {
            return ReadTareWeightResponse.UnpackResponseMessage(this, responseMessage);
        }
#else
        public ReadTareWeightResponse UnpackResponseMessage(Memory<byte> responseMessage)
        {
            return ReadTareWeightResponse.UnpackResponseMessage(this, responseMessage);
        }
#endif

        public static ReadTareWeightRequest CreateNew(MettlerToledoDevice device)
        {
            return new ReadTareWeightRequest(Commands.ReadTareWeight);
        }

        #endregion


        #region Protected Methods

        protected override void BuildMessageDetail(ref StringBuilder messageBuilder)
        {
        }

        #endregion
    }
}
