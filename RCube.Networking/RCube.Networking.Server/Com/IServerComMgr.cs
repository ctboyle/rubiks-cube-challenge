using System;
using System.Net;
using RCube.Networking.Messages;

namespace RCube.Networking.Server.Communications
{
    internal interface IServerComMgr
    {       
        bool IsRunning { get; }
        void Start();
        void Stop();
        void SendToClient(EndPoint ep, DataMessage msg);
    }
}
