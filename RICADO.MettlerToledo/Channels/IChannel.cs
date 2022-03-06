using System;
using System.Threading;
using System.Threading.Tasks;

namespace RICADO.MettlerToledo.Channels
{
    internal interface IChannel : IDisposable
    {
        Task InitializeAsync(int timeout, CancellationToken cancellationToken);

#if NETSTANDARD
        Task<ProcessMessageResult> ProcessMessageAsync(byte[] requestMessage, ProtocolType protocol, int timeout, int retries, CancellationToken cancellationToken);
#else
        Task<ProcessMessageResult> ProcessMessageAsync(ReadOnlyMemory<byte> requestMessage, ProtocolType protocol, int timeout, int retries, CancellationToken cancellationToken);
#endif
    }
}
