using System;

namespace RICADO.MettlerToledo
{
    public class SICSException : Exception
    {
        #region Constructors

        internal SICSException(string message) : base(message)
        {
        }

        internal SICSException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
