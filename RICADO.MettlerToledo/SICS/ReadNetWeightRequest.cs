using System;
using System.Text;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadNetWeightRequest : Request
    {
        #region Constructor

        protected ReadNetWeightRequest(string commandCode) : base(commandCode)
        {
        }

        #endregion


        #region Public Methods

#if NETSTANDARD
        public ReadNetWeightResponse UnpackResponseMessage(byte[] responseMessage)
        {
            return ReadNetWeightResponse.UnpackResponseMessage(this, responseMessage);
        }
#else
        public ReadNetWeightResponse UnpackResponseMessage(Memory<byte> responseMessage)
        {
            return ReadNetWeightResponse.UnpackResponseMessage(this, responseMessage);
        }
#endif

        public static ReadNetWeightRequest CreateNew(MettlerToledoDevice device)
        {
            return new ReadNetWeightRequest(Commands.ReadNetWeight);
        }

        #endregion


        #region Protected Methods

        protected override void BuildMessageDetail(ref StringBuilder messageBuilder)
        {
        }

        #endregion
    }
}
