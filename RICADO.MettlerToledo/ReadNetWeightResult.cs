using System;

namespace RICADO.MettlerToledo
{
    public class ReadNetWeightResult : RequestResult
    {
        #region Private Fields

        private readonly double _netWeight;
        private readonly string _units;

        #endregion


        #region Public Properties

        public double NetWeight => _netWeight;
        public string Units => _units;

        #endregion


        #region Constructor

        internal ReadNetWeightResult(Channels.ProcessMessageResult result, double netWeight, string units) : base(result)
        {
            _netWeight = netWeight;
            _units = units;
        }

        #endregion
    }
}
