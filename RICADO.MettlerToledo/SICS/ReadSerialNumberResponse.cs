using System;
using System.Text.RegularExpressions;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadSerialNumberResponse : Response
    {
        #region Constants

        private const string MessageRegex = "^I4 A \"([0-9A-Za-z]+)\"$";

        #endregion


        #region Private Fields

        private string _serialNumber;

        #endregion


        #region Public Properties

        public string SerialNumber => _serialNumber;

        #endregion


        #region Constructor

#if NETSTANDARD
        protected ReadSerialNumberResponse(Request request, byte[] responseMessage) : base(request, responseMessage)
        {
        }
#else
        protected ReadSerialNumberResponse(Request request, Memory<byte> responseMessage) : base(request, responseMessage)
        {
        }
#endif

        #endregion


        #region Public Methods

#if NETSTANDARD
        public static ReadSerialNumberResponse UnpackResponseMessage(ReadSerialNumberRequest request, byte[] responseMessage)
        {
            return new ReadSerialNumberResponse(request, responseMessage);
        }
#else
        public static ReadSerialNumberResponse UnpackResponseMessage(ReadSerialNumberRequest request, Memory<byte> responseMessage)
        {
            return new ReadSerialNumberResponse(request, responseMessage);
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
                throw new SICSException("The Read Serial Number Response Message Format was Invalid");
            }

            _serialNumber = regexSplit[1];
        }

        #endregion
    }
}
