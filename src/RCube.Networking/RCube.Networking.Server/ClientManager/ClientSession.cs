using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Server.ClientMangement
{
    internal class ClientSession
    {
        private Guid _sessionId;
        private EndPoint _clientEp;

        public ClientSession() :
            this(Guid.Empty, new IPEndPoint(IPAddress.Any, 0))
        {
        }

        private ClientSession(Guid sessionId, EndPoint ep)
        {
            SessionId = sessionId;
            ClientEndPoint = ep;
        }

        public Guid SessionId
        {
            get { return _sessionId; }
            set { _sessionId = value; }
        }

        public EndPoint ClientEndPoint
        {
            get { return _clientEp; }
            set { _clientEp = value; }
        }
    }
}
