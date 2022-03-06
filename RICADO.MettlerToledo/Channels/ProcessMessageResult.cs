using System;

namespace RICADO.MettlerToledo.Channels
{
    internal struct ProcessMessageResult
    {
        internal int BytesSent;
        internal int PacketsSent;
        internal int BytesReceived;
        internal int PacketsReceived;
        internal double Duration;

#if NETSTANDARD
        internal byte[] ResponseMessage;
#else
        internal Memory<byte> ResponseMessage;
#endif
    }
}
