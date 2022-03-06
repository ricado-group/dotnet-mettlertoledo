using System;

namespace RICADO.MettlerToledo.SICS
{
    internal static class Commands
    {
        #region Public Properties

        public static readonly string ReadWeightAndStatus = "SIX1";
        public static readonly string ReadTareWeight = "TA";
        public static readonly string ReadNetWeight = "SI";
        public static readonly string ZeroStableCommand = "Z";
        public static readonly string ZeroImmediatelyCommand = "ZI";
        public static readonly string TareStableCommand = "T";
        public static readonly string TareImmediatelyCommand = "TI";
        public static readonly string ClearTareCommand = "TAC";
        public static readonly string ReadFirmwareRevision = "I3";
        public static readonly string ReadSerialNumber = "I4";

        #endregion
    }
}
