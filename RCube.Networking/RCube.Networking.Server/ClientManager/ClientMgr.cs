using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Threading;

namespace RCube.Networking.Server.ClientMangement
{
    internal sealed class ClientMgr : IClientMgr
    {
        private Dictionary<Guid, ClientSession> _registeredClients;
        private IServiceProvider _svcProvider;

        public ClientMgr(IServiceProvider serviceProvider)
        {
            _registeredClients = new Dictionary<Guid, ClientSession>();
            _svcProvider = serviceProvider;
        }

        public ClientSession GetClientSession(Guid id)
        {
            return _registeredClients[id];
        }

        public ClientSession RegisterClient(EndPoint clientEp)
        {
            Guid clientId = Guid.Empty;
            ClientSession clientSession = new ClientSession();

            do
            {
                clientId = Guid.NewGuid();
            } while (_registeredClients.ContainsKey(clientId));

            clientSession.SessionId = clientId;
            clientSession.ClientEndPoint = clientEp;
            _registeredClients.Add(clientId, clientSession);

            return clientSession;
        }

        public void UnregisterClient(Guid guid)
        {
            if (!_registeredClients.ContainsKey(guid))
                throw new Exception("Cannot unregister the client because the specified client id is not registered.");

            _registeredClients.Remove(guid);
        }
    }
}
