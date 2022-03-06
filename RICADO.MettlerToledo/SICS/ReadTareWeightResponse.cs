using System;
using System.Text.RegularExpressions;

namespace RICADO.MettlerToledo.SICS
{
    internal class ReadTareWeightResponse : Response
    {
        #region Constants

        private const string SuccessMessageRegex = "^TA A\u0020+([0-9\u002E\\-]+) (.*)$";
        private const string FailureMessageRegex = "^TA [IL]";

        #endregion


        #region Private Fields

        private double _tareWeight;
        private string _units;

        #endregion


        #region Public Properties

        public double TareWeight => _tareWeight;
        public string Units => _units;

        #endregion


        #region Constructor

#if NETSTANDARD
        protected ReadTareWeightResponse(Request request, byte[] responseMessage) : base(request, responseMessage)
        {
        }
#else
        protected ReadTareWeightResponse(Request request, Memory<byte> responseMessage) : base(request, responseMessage)
        {
        }
#endif

        #endregion


        #region Public Methods

#if NETSTANDARD
        public static ReadTareWeightResponse UnpackResponseMessage(ReadTareWeightRequest request, byte[] responseMessage)
        {
            return new ReadTareWeightResponse(request, responseMessage);
        }
#else
        public static ReadTareWeightResponse UnpackResponseMessage(ReadTareWeightRequest request, Memory<byte> responseMessage)
        {
            return new ReadTareWeightResponse(request, responseMessage);
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
            else if(Regex.IsMatch(messageDetail, FailureMessageRegex))
            {
                throw new SICSException("The Read Tare Weight Request cannot be Executed at this Time");
            }
            else
            {
                throw new SICSException("The Read Tare Weight Response Message Format was Invalid");
            }

            double weight;

            if(double.TryParse(regexSplit[1], out weight) == false)
            {
                throw new SICSException("Failed to Extract a Weight Value from the Read Tare Weight Response");
            }

            _tareWeight = weight;

            _units = regexSplit[2];
        }

        #endregion
    }
}
