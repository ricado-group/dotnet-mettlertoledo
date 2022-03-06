using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RICADO.Sockets;

namespace RICADO.MettlerToledo.Channels
{
    internal class EthernetChannel : IChannel
    {
        #region Private Fields

        private readonly string _remoteHost;
        private readonly int _port;

        private TcpClient _client;

        private readonly SemaphoreSlim _semaphore;

        #endregion


        #region Internal Properties

        internal string RemoteHost => _remoteHost;

        internal int Port => _port;

        #endregion


        #region Constructors

        internal EthernetChannel(string remoteHost, int port)
        {
            _remoteHost = remoteHost;
            _port = port;

            _semaphore = new SemaphoreSlim(1, 1);
        }

        #endregion


        #region Public Methods

        public void Dispose()
        {
            try
            {
                _client?.Dispose();
            }
            catch
            {
            }
            finally
            {
                _client = null;
            }

            _semaphore.Dispose();
        }

        #endregion


        #region Internal Methods

        public async Task InitializeAsync(int timeout, CancellationToken cancellationToken)
        {
            if (!_semaphore.Wait(0))
            {
                await _semaphore.WaitAsync(cancellationToken);
            }

            try
            {
                destroyClient();

                await initializeClient(timeout, cancellationToken);
            }
            finally
            {
                _semaphore.Release();
            }
        }

#if NETSTANDARD
        public async Task<ProcessMessageResult> ProcessMessageAsync(byte[] requestMessage, ProtocolType protocol, int timeout, int retries, CancellationToken cancellationToken)
#else
        public async Task<ProcessMessageResult> ProcessMessageAsync(ReadOnlyMemory<byte> requestMessage, ProtocolType protocol, int timeout, int retries, CancellationToken cancellationToken)
#endif
        {
            int attempts = 0;
            int bytesSent = 0;
            int packetsSent = 0;
            int bytesReceived = 0;
            int packetsReceived = 0;
            DateTime startTimestamp = DateTime.UtcNow;

#if NETSTANDARD
            byte[] responseMessage = new byte[0];
#else
            Memory<byte> responseMessage = new Memory<byte>();
#endif

            while (attempts <= retries)
            {
                if (!_semaphore.Wait(0))
                {
                    await _semaphore.WaitAsync(cancellationToken);
                }

                try
                {
                    if (attempts > 0)
                    {
                        await destroyAndInitializeClient(timeout, cancellationToken);
                    }

                    // Send the Message
                    SendMessageResult sendResult = await sendMessageAsync(requestMessage, protocol, timeout, cancellationToken);

                    bytesSent += sendResult.Bytes;
                    packetsSent += sendResult.Packets;

                    // Receive a Response
                    ReceiveMessageResult receiveResult = await receiveMessageAsync(protocol, timeout, cancellationToken);

                    bytesReceived += receiveResult.Bytes;
                    packetsReceived += receiveResult.Packets;
                    responseMessage = receiveResult.Message;

                    break;
                }
                catch (Exception)
                {
                    if (attempts >= retries)
                    {
                        throw;
                    }
                }
                finally
                {
                    _semaphore.Release();
                }

                // Increment the Attempts
                attempts++;
            }

            return new ProcessMessageResult
            {
                BytesSent = bytesSent,
                PacketsSent = packetsSent,
                BytesReceived = bytesReceived,
                PacketsReceived = packetsReceived,
                Duration = DateTime.UtcNow.Subtract(startTimestamp).TotalMilliseconds,
                ResponseMessage = responseMessage,
            };
        }

        #endregion


        #region Private Methods

        private Task initializeClient(int timeout, CancellationToken cancellationToken)
        {
            _client = new TcpClient(RemoteHost, Port);

            return _client.ConnectAsync(timeout, cancellationToken);
        }

        private void destroyClient()
        {
            try
            {
                _client?.Dispose();
            }
            finally
            {
                _client = null;
            }
        }

        private async Task destroyAndInitializeClient(int timeout, CancellationToken cancellationToken)
        {
            destroyClient();

            try
            {
                await initializeClient(timeout, cancellationToken);
            }
            catch (ObjectDisposedException)
            {
                throw new MettlerToledoException("Failed to Re-Connect to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "' - The underlying Socket Connection has been Closed");
            }
            catch (TimeoutException)
            {
                throw new MettlerToledoException("Failed to Re-Connect within the Timeout Period to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "'");
            }
            catch (System.Net.Sockets.SocketException e)
            {
                throw new MettlerToledoException("Failed to Re-Connect to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "'", e);
            }
        }

#if NETSTANDARD
        private async Task<SendMessageResult> sendMessageAsync(byte[] message, ProtocolType protocol, int timeout, CancellationToken cancellationToken)
#else
        private async Task<SendMessageResult> sendMessageAsync(ReadOnlyMemory<byte> message, ProtocolType protocol, int timeout, CancellationToken cancellationToken)
#endif
        {
            SendMessageResult result = new SendMessageResult
            {
                Bytes = 0,
                Packets = 0,
            };

            if(_client == null)
            {
                throw new MettlerToledoException("Failed to Send " + protocol + " Message to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "' - The underlying Socket Connection has been Closed");
            }

            try
            {
                result.Bytes += await _client.SendAsync(message, timeout, cancellationToken);
                result.Packets += 1;
            }
            catch (ObjectDisposedException)
            {
                throw new MettlerToledoException("Failed to Send " + protocol + " Message to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "' - The underlying Socket Connection has been Closed");
            }
            catch (TimeoutException)
            {
                throw new MettlerToledoException("Failed to Send " + protocol + " Message within the Timeout Period to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "'");
            }
            catch (System.Net.Sockets.SocketException e)
            {
                throw new MettlerToledoException("Failed to Send " + protocol + " Message to Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "'", e);
            }

            return result;
        }

        private async Task<ReceiveMessageResult> receiveMessageAsync(ProtocolType protocol, int timeout, CancellationToken cancellationToken)
        {
#if NETSTANDARD
            ReceiveMessageResult result = new ReceiveMessageResult
            {
                Bytes = 0,
                Packets = 0,
                Message = new byte[0],
            };
#else
            ReceiveMessageResult result = new ReceiveMessageResult
            {
                Bytes = 0,
                Packets = 0,
                Message = new Memory<byte>(),
            };
#endif

            if(_client == null)
            {
                throw new MettlerToledoException("Failed to Receive " + protocol + " Message from Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "' - The underlying Socket Connection has been Closed");
            }

            try
            {
                List<byte> receivedData = new List<byte>();
                DateTime startTimestamp = DateTime.UtcNow;

                bool receiveCompleted = false;

                while (DateTime.UtcNow.Subtract(startTimestamp).TotalMilliseconds < timeout && receiveCompleted == false)
                {
#if NETSTANDARD
                    byte[] buffer = new byte[100];
#else
                    Memory<byte> buffer = new byte[100];
#endif

                    TimeSpan receiveTimeout = TimeSpan.FromMilliseconds(timeout).Subtract(DateTime.UtcNow.Subtract(startTimestamp));

                    if (receiveTimeout.TotalMilliseconds >= 50)
                    {
                        int receivedBytes = await _client.ReceiveAsync(buffer, receiveTimeout, cancellationToken);

                        if (receivedBytes > 0)
                        {
#if NETSTANDARD
                            receivedData.AddRange(buffer.Take(receivedBytes));
#else
                            receivedData.AddRange(buffer.Slice(0, receivedBytes).ToArray());
#endif

                            result.Bytes += receivedBytes;
                            result.Packets += 1;
                        }
                    }

                    receiveCompleted = isReceiveCompleted(protocol, receivedData);
                }

                if (receivedData.Count == 0)
                {
                    throw new MettlerToledoException("Failed to Receive " + protocol + " Message from Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "' - No Data was Received");
                }

                if (receiveCompleted == false)
                {
                    throw new MettlerToledoException("Failed to Receive " + protocol + " Message within the Timeout Period from Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "'");
                }

                result.Message = trimReceivedData(protocol, receivedData);
            }
            catch (ObjectDisposedException)
            {
                throw new MettlerToledoException("Failed to Receive " + protocol + " Message from Mettler Toledo Ethernet Device '" + RemoteHost + ":" + Port + "' - The underlying Socket Connection has been Closed");
            }
            catch (TimeoutException)
            {
                throw new MettlerToledoException("Failed to Receive " + protocol + " Message within the Timeout Period from Mettler Toledo Ethernet Device  '" + RemoteHost + ":" + Port + "'");
            }
            catch (System.Net.Sockets.SocketException e)
            {
                throw new MettlerToledoException("Failed to Receive " + protocol + " Message from Mettler Toledo Ethernet Device  '" + RemoteHost + ":" + Port + "'", e);
            }

            return result;
        }

        private bool isReceiveCompleted(ProtocolType protocol, List<byte> receivedData)
        {
            if (receivedData.Count == 0)
            {
                return false;
            }

            if(protocol != ProtocolType.SICS)
            {
                return false;
            }

            if (receivedData.HasSequence(SICS.Response.ETX) == false)
            {
                return false;
            }

            return true;
        }

#if NETSTANDARD
        private byte[] trimReceivedData(ProtocolType protocol, List<byte> receivedData)
#else
        private Memory<byte> trimReceivedData(ProtocolType protocol, List<byte> receivedData)
#endif
        {
            if (receivedData.Count == 0)
            {
#if NETSTANDARD
                return Array.Empty<byte>();
#else
                return Memory<byte>.Empty;
#endif
            }

            byte[] etxBytes = protocol == ProtocolType.SICS ? SICS.Response.ETX : new byte[0];

            int etxIndex = receivedData.IndexOf(etxBytes);

            return receivedData.Take(etxIndex + etxBytes.Length).ToArray();
        }

        #endregion
    }
}
