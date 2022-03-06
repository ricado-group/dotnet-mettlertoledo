using System;

namespace RICADO.MettlerToledo
{
    public class ReadSerialNumberResult : RequestResult
    {
        #region Private Fields

        private readonly string _serialNumber;

        #endregion


        #region Public Properties

        public string SerialNumber => _serialNumber;

        #endregion


        #region Constructor

        internal ReadSerialNumberResult(Channels.ProcessMessageResult result, string serialNumber) : base(result)
        {
            _serialNumber = serialNumber;
        }

        #endregion
    }
}
