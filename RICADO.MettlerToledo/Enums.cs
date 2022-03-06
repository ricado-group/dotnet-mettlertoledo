using System;

namespace RICADO.MettlerToledo
{
    public enum CommandType
    {
        ZeroStable,
        ZeroImmediately,
        TareStable,
        TareImmediately,
        ClearTare,
    }

    public enum WeightType
    {
        Gross,
        Tare,
        Net
    }

    public enum ConnectionMethod
    {
        Ethernet
    }

    public enum ProtocolType
    {
        SICS
    }
}
