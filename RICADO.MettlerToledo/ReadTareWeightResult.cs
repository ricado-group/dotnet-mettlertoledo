using System;

namespace RICADO.MettlerToledo
{
    public class ReadTareWeightResult : RequestResult
    {
        #region Private Fields

        private readonly double _tareWeight;
        private readonly string _units;

        #endregion


        #region Public Properties

        public double TareWeight => _tareWeight;
        public string Units => _units;

        #endregion


        #region Constructor

        internal ReadTareWeightResult(Channels.ProcessMessageResult result, double tareWeight, string units) : base(result)
        {
            _tareWeight = tareWeight;
            _units = units;
        }

        #endregion
    }
}
