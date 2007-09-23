using System;
using System.Collections.Generic;
using System.Text;
using RCube.Networking.Messages;
using System.Net;
using System.Threading;
using NSpring.Logging;
using RCube.Networking.Server.MessageManagement.Handlers;
using RCube.Networking.Server.Communications;

namespace RCube.Networking.Server.MessageManagement
{
    internal sealed class MessageMgr : IMessageMgr
    {
        private Dictionary<Type, IMessageHandler> _msgHandlers;
        private Dictionary<Guid, OpenMessage> _openResponses;
        private IServiceProvider _svcProvider;

        public MessageMgr(IServiceProvider svcProvider)
        {
            _msgHandlers = new Dictionary<Type, IMessageHandler>();
            _openResponses = new Dictionary<Guid, OpenMessage>();
            _svcProvider = svcProvider;
        }

        private Logger Logger
        {
            get { return _svcProvider.GetService(typeof(Logger)) as Logger; }
        }

        private IServerComMgr ServerComMgr
        {
            get { return _svcProvider.GetService(typeof(IServerComMgr)) as IServerComMgr; }
        }

        public void RegisterMessageHandler(Type msgType, IMessageHandler handler)
        {
            if(!msgType.IsSubclassOf(typeof(DataMessage)))
                throw new Exception("Unable to add message handler for that type.");

            _msgHandlers.Add(msgType, handler);
        }

        public void UnregisterMessageHandler(Type msgType)
        {
            _msgHandlers.Remove(msgType);
        }

        public void SendMessage(EndPoint ep, DataMessage msg)
        {
            ServerComMgr.SendToClient(ep, msg);
        }

        public void SendMessageWithResponse(EndPoint ep, OpenMessage msg, int timeout)
        {
            Guid msgId = msg.Message.Header.MessageId;

            Thread timeoutThread = new Thread(new ThreadStart(
                delegate()
                {
                    Thread.Sleep(timeout);

                    lock (_openResponses)
                    {
                        if (_openResponses.ContainsKey(msgId))
                        {
                            _openResponses[msgId].TimeoutCallback.Invoke(msg.Message);
                            _openResponses.Remove(msgId);
                        }
                    }
                }
            ));

            lock (_openResponses)
            {
                SendMessage(ep, msg.Message);
                _openResponses.Add(msgId, msg);
                timeoutThread.Start();
            }
        }

        public void RecieveMessage(EndPoint ep, DataMessage msg)
        {
            Guid id = msg.Header.MessageId;
            OpenMessage openMsg = null;

            lock (_openResponses)
            {
                if (_openResponses.ContainsKey(id))
                {
                    openMsg = _openResponses[id];
                    _openResponses.Remove(id);
                }
            }

            if (openMsg == null)
            {
                Type type = msg.GetType();

                if(_msgHandlers.ContainsKey(type))
                    _msgHandlers[type].HandleMessage(_svcProvider, ep, msg);
            }
            else
            {
                openMsg.RecieptCallback.Invoke(msg);
            }
        }
    }
}
