using System;

namespace RICADO.MettlerToledo
{
    public class ReadWeightAndStatusResult : RequestResult
    {
        #region Private Fields

        private readonly bool _stableStatus;
        private readonly bool _centerOfZero;
        private readonly double _grossWeight;
        private readonly double _netWeight;
        private readonly double _tareWeight;
        private readonly string _units;

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

        internal ReadWeightAndStatusResult(Channels.ProcessMessageResult result, bool stableStatus, bool centerOfZero, double grossWeight, double netWeight, double tareWeight, string units) : base(result)
        {
            _stableStatus = stableStatus;
            _centerOfZero = centerOfZero;
            _grossWeight = grossWeight;
            _netWeight = netWeight;
            _tareWeight = tareWeight;
            _units = units;
        }

        #endregion
    }
}
