using Mirror;
using System;

namespace MVPMusicBox.MusicAPI
{
    internal class FakeConnection : NetworkConnectionToClient
    {
        public FakeConnection(int networkConnectionId) : base(networkConnectionId)
        {

        }

        public override string address => "localhost";

        public override void Send(ArraySegment<byte> segment, int channelId = 0)
        {

        }

        public override void Disconnect()
        {

        }
    }
}
