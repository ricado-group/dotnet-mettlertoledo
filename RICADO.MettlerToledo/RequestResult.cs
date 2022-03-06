using System;

namespace RICADO.MettlerToledo
{
    public abstract class RequestResult
    {
        #region Private Fields

        public int _bytesSent = 0;
        public int _packetsSent = 0;
        public int _bytesReceived = 0;
        public int _packetsReceived = 0;
        public double _duration = 0;

        #endregion


        #region Public Properties

        public int BytesSent => _bytesSent;
        public int PacketsSent => _packetsReceived;
        public int BytesReceived => _bytesReceived;
        public int PacketsReceived => _packetsReceived;
        public double Duration => _duration;

        #endregion


        #region Constructor

        internal RequestResult()
        {
        }

        internal RequestResult(Channels.ProcessMessageResult result)
        {
            _bytesSent = result.BytesSent;
            _packetsSent = result.PacketsSent;
            _bytesReceived = result.BytesReceived;
            _packetsReceived = result.PacketsReceived;
            _duration = result.Duration;
        }

        #endregion


        #region Internal Methods

        internal void AddMessageResult(Channels.ProcessMessageResult result)
        {
            _bytesSent += result.BytesSent;
            _packetsSent += result.PacketsSent;
            _bytesReceived += result.BytesReceived;
            _packetsReceived += result.PacketsReceived;
            _duration += result.Duration;
        }

        #endregion
    }
}
