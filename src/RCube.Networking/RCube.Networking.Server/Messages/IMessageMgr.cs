using System;
using System.Collections.Generic;
using System.Text;
using RCube.Networking.Messages;
using System.Net;
using RCube.Networking.Server.MessageManagement.Handlers;

namespace RCube.Networking.Server.MessageManagement
{
    public delegate void MessageCallback(DataMessage msg);

    internal class OpenMessage
    {
        public DataMessage Message;
        public MessageCallback TimeoutCallback;
        public MessageCallback RecieptCallback;
    }

    internal interface IMessageMgr
    {
        
        void RegisterMessageHandler(Type msgType, IMessageHandler handler);
        void UnregisterMessageHandler(Type msgType);
        void SendMessage(EndPoint ep, DataMessage msg);
        void SendMessageWithResponse(EndPoint ep, OpenMessage msg, int timeout);
        void RecieveMessage(EndPoint ep, DataMessage msg);
    }
}
