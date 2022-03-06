using System;

namespace RICADO.MettlerToledo
{
    public class MettlerToledoException : Exception
    {
        #region Constructors

        internal MettlerToledoException(string message) : base(message)
        {
        }

        internal MettlerToledoException(string message, Exception innerException) : base(message, innerException)
        {
        }

        #endregion
    }
}
