using System;
using System.Collections.Generic;
using System.Text;
using RCube.Networking.Messages;
using System.Net;

namespace RCube.Networking.Server.MessageManagement.Handlers
{
    internal interface IMessageHandler
    {
        void HandleMessage(IServiceProvider svcProvider, EndPoint ep, DataMessage msg);
    }
}
