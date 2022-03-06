using System;
using System.Text.RegularExpressions;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadFirmwareRevisionResponse : Response
    {
        #region Constants

        private const string MessageRegex = "^I3 A \"([0-9A-Za-z\u002E]+)\"$";

        #endregion


        #region Private Fields

        private string _version;

        #endregion


        #region Public Properties

        public string Version => _version;

        #endregion


        #region Constructor

#if NETSTANDARD
        protected ReadFirmwareRevisionResponse(Request request, byte[] responseMessage) : base(request, responseMessage)
        {
        }
#else
        protected ReadFirmwareRevisionResponse(Request request, Memory<byte> responseMessage) : base(request, responseMessage)
        {
        }
#endif

        #endregion


        #region Public Methods

#if NETSTANDARD
        public static ReadFirmwareRevisionResponse UnpackResponseMessage(ReadFirmwareRevisionRequest request, byte[] responseMessage)
        {
            return new ReadFirmwareRevisionResponse(request, responseMessage);
        }
#else
        public static ReadFirmwareRevisionResponse UnpackResponseMessage(ReadFirmwareRevisionRequest request, Memory<byte> responseMessage)
        {
            return new ReadFirmwareRevisionResponse(request, responseMessage);
        }
#endif

        #endregion


        #region Protected Methods

        protected override void UnpackMessageDetail(Request request, string messageDetail)
        {
            string[] regexSplit;

            if (Regex.IsMatch(messageDetail, MessageRegex))
            {
                regexSplit = Regex.Split(messageDetail, MessageRegex);
            }
            else
            {
                throw new SICSException("The Read Firmware Revision Response Message Format was Invalid");
            }

            _version = regexSplit[1];
        }

        #endregion
    }
}
