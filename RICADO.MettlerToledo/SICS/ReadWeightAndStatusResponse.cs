using System;
using System.Text.RegularExpressions;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadWeightAndStatusResponse : Response
    {
        #region Constants

        private const string SuccessMessageRegex = "^SIX1 ([SD]) 0 ([ZN]) [RN] R 0 0 0 1 [NMP] ([0-9\u0020\u002E\\-]{9}) ([0-9\u0020\u002E\\-]{9}) ([0-9\u0020\u002E\\-]{9}) (.*)$";
        private const string OutOfRangeMessageRegex = "^SIX1 [\u002B\u002B\\-]";
        private const string FailureMessageRegex = "^SIX1 [/I]";

        #endregion


        #region Private Fields

        private bool _stableStatus;
        private bool _centerOfZero;
        private double _grossWeight;
        private double _netWeight;
        private double _tareWeight;
        private string _units;

        #endregion


        #region Public Properties

        public bool StableStatus => _stableStatus;
        public bool CenterOfZero => _centerOfZero;
        public double GrossWeight => _grossWeight;
        public double NetWeight => _netWeight;
        public double TareWeight => _tareWeight;
        public string Units => _units;

        #endregion


        #region Constructor

#if NETSTANDARD
        protected ReadWeightAndStatusResponse(Request request, byte[] responseMessage) : base(request, responseMessage)
        {
        }
#else
        protected ReadWeightAndStatusResponse(Request request, Memory<byte> responseMessage) : base(request, responseMessage)
        {
        }
#endif

        #endregion


        #region Public Methods

#if NETSTANDARD
        public static ReadWeightAndStatusResponse UnpackResponseMessage(ReadWeightAndStatusRequest request, byte[] responseMessage)
        {
            return new ReadWeightAndStatusResponse(request, responseMessage);
        }
#else
        public static ReadWeightAndStatusResponse UnpackResponseMessage(ReadWeightAndStatusRequest request, Memory<byte> responseMessage)
        {
            return new ReadWeightAndStatusResponse(request, responseMessage);
        }
#endif

        #endregion


        #region Protected Methods

        protected override void UnpackMessageDetail(Request request, string messageDetail)
        {
            string[] regexSplit;

            if (Regex.IsMatch(messageDetail, SuccessMessageRegex))
            {
                regexSplit = Regex.Split(messageDetail, SuccessMessageRegex);
            }
            else if (Regex.IsMatch(messageDetail, OutOfRangeMessageRegex))
            {
                throw new SICSException("Failed to Read the Weight and Status. The Scale is Out of Range");
            }
            else if (Regex.IsMatch(messageDetail, FailureMessageRegex))
            {
                throw new SICSException("Failed to Read the Weight and Status. The Response included an Inclination or Invalid Value");
            }
            else
            {
                throw new SICSException("The Read Weight and Status Response Message Format was Invalid");
            }

            _stableStatus = regexSplit[1] == "S";

            _centerOfZero = regexSplit[2] == "Z";

            if(double.TryParse(regexSplit[3], out double grossWeight) == false)
            {
                throw new SICSException("Failed to Read the Weight and Status. The Gross Weight was Invalid");
            }

            _grossWeight = grossWeight;

            if (double.TryParse(regexSplit[4], out double netWeight) == false)
            {
                throw new SICSException("Failed to Read the Weight and Status. The Net Weight was Invalid");
            }

            _netWeight = netWeight;

            if (double.TryParse(regexSplit[5], out double tareWeight) == false)
            {
                throw new SICSException("Failed to Read the Weight and Status. The Tare Weight was Invalid");
            }

            _tareWeight = tareWeight;

            _units = regexSplit[6];
        }

        #endregion
    }
}
