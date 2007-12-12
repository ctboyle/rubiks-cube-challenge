using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Server.ClientMangement
{
    internal interface IClientMgr
    {
        ClientSession GetClientSession(Guid id);
        ClientSession RegisterClient(EndPoint clientEp);
        void UnregisterClient(Guid guid);
    }
}
