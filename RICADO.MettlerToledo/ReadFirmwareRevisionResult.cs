using System;

namespace RICADO.MettlerToledo
{
    public class ReadFirmwareRevisionResult : RequestResult
    {
        #region Private Fields

        private readonly string _version;

        #endregion


        #region Public Properties

        public string Version => _version;

        #endregion


        #region Constructor

        internal ReadFirmwareRevisionResult(Channels.ProcessMessageResult result, string version) : base(result)
        {
            _version = version;
        }

        #endregion
    }
}
