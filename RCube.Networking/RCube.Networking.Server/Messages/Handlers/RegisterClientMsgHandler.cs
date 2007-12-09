using System;
using System.Collections.Generic;
using System.Text;
using RCube.Networking.Messages;
using System.Net;
using RCube.Networking.Server.ClientMangement;

namespace RCube.Networking.Server.MessageManagement.Handlers
{
    internal sealed class RegisterClientMsgHandler : IMessageHandler
    {
        #region IMessageHandler Members

        public void HandleMessage(IServiceProvider svcProvider, EndPoint ep, DataMessage msg)
        {
            RegisterClientMessage realMsg = msg as RegisterClientMessage;
            IClientMgr clientMgr = svcProvider.GetService(typeof(IClientMgr)) as IClientMgr;
            ClientSession cs = null;

            switch (realMsg.Type)
            {
                case RegisterClientMessage.RegistrationType.Register:
                    cs = clientMgr.RegisterClient(ep);
                    break;
                case RegisterClientMessage.RegistrationType.Unregister:
                    cs = clientMgr.GetClientSession(msg.Header.SessionId);
                    if (cs == null) return;
                    clientMgr.UnregisterClient(msg.Header.SessionId);
                    break;
            }

            if (realMsg.NeedsAcknowledgement)
            {
                IMessageMgr msgMgr = svcProvider.GetService(typeof(IMessageMgr)) as IMessageMgr;

                DataMessageHeader hdr = new DataMessageHeader();
                hdr.SessionId = cs.SessionId;
                hdr.Timestamp = DateTime.Now;
                hdr.MessageId = Guid.Empty;

                RegisterClientMessage respMsg = new RegisterClientMessage(hdr);
                respMsg.NeedsAcknowledgement = false;
                respMsg.Type = realMsg.Type;

                msgMgr.SendMessage(cs.ClientEndPoint, respMsg);
            }
        }

        #endregion
    }
}
