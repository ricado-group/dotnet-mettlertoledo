using System;

namespace RICADO.MettlerToledo.Channels
{
    internal struct ReceiveMessageResult
    {
#if NETSTANDARD
        internal byte[] Message;
#else
        internal Memory<byte> Message;
#endif
        internal int Bytes;
        internal int Packets;
    }
}
