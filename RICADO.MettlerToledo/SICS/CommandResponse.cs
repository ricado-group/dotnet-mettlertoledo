using System;
using System.Text.RegularExpressions;

namespace RICADO.MettlerToledo.SICS
{
    internal class CommandResponse : Response
    {
        #region Constants

        private const string SuccessMessageRegex = "^(Z|ZI|T|TI|TAC) [ADS]";
        private const string OutOfRangeMessageRegex = "^(Z|ZI|T|TI|TAC) [\u002B\u002D\\-]";
        private const string FailureMessageRegex = "^(Z|ZI|T|TI|TAC) [I]";

        #endregion


        #region Constructor

#if NETSTANDARD
        protected CommandResponse(Request request, byte[] responseMessage) : base(request, responseMessage)
        {
        }
#else
        protected CommandResponse(Request request, Memory<byte> responseMessage) : base(request, responseMessage)
        {
        }
#endif

        #endregion


        #region Public Methods

#if NETSTANDARD
        public static void ValidateResponseMessage(CommandRequest request, byte[] responseMessage)
        {
            _ = new CommandResponse(request, responseMessage);
        }
#else
        public static void ValidateResponseMessage(CommandRequest request, Memory<byte> responseMessage)
        {
            _ = new CommandResponse(request, responseMessage);
        }
#endif

        #endregion


        #region Protected Methods

        protected override void UnpackMessageDetail(Request request, string messageDetail)
        {
            if (request is CommandRequest commandRequest)
            {
                string[] regexSplit;

                if (Regex.IsMatch(messageDetail, SuccessMessageRegex))
                {
                    regexSplit = Regex.Split(messageDetail, SuccessMessageRegex);
                }
                else if (Regex.IsMatch(messageDetail, OutOfRangeMessageRegex))
                {
                    throw new SICSException("Failed to Execute the '" + commandRequest.Command.ToString() + "' Command. The Scale Reading is Out of Range");
                }
                else if (Regex.IsMatch(messageDetail, FailureMessageRegex))
                {
                    throw new SICSException("Failed to Execute the '" + commandRequest.Command.ToString() + "' Command. The Command cannot be Executed at this Time");
                }
                else
                {
                    throw new SICSException("The Command Response Message Format was Invalid");
                }

                bool throwException = false;

                switch (commandRequest.Command)
                {
                    case CommandType.ZeroStable:
                        if (regexSplit[1] != "Z")
                        {
                            throwException = true;
                        }
                        break;

                    case CommandType.ZeroImmediately:
                        if (regexSplit[1] != "Z" && regexSplit[1] != "ZI") // Mettler has a bug in the Response Code (or bad documentation)
                        {
                            throwException = true;
                        }
                        break;

                    case CommandType.TareStable:
                        if (regexSplit[1] != "T")
                        {
                            throwException = true;
                        }
                        break;

                    case CommandType.TareImmediately:
                        if (regexSplit[1] != "T" && regexSplit[1] != "TI") // Mettler has a bug in the Response Code (or bad documentation)
                        {
                            throwException = true;
                        }
                        break;

                    case CommandType.ClearTare:
                        if (regexSplit[1] != "TAC")
                        {
                            throwException = true;
                        }
                        break;
                }

                if (throwException == true)
                {
                    throw new SICSException("Failed to Execute the '" + commandRequest.Command.ToString() + "' Command. The Response Data was for a different Command");
                }
            }
        }

        #endregion
    }
}
