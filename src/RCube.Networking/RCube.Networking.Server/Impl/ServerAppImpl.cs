using System;
using System.Collections.Generic;
using System.Text;
using NSpring.Logging;
using System.Threading;
using NSpring.Logging.Loggers;
using RCube.Networking.Messages;
using RCube.Networking.Server.Communications;
using RCube.Networking.Server.ClientMangement;
using RCube.Networking.Server.MessageManagement;
using RCube.Networking.Server.MessageManagement.Handlers;

namespace RCube.Networking.Server.Impl
{
    internal sealed class ServerAppImpl : ServerApp
    {
        Logger _logger;
        IServerComMgr _comMgr;
        IClientMgr _clientMgr;
        IMessageMgr _msgMgr;

        protected override void Initialize()
        {
            _msgMgr = new MessageMgr(Services);
            _comMgr = new ServerComMgr(Services);
            _clientMgr = new ClientMgr(Services);
            _logger = new CompositeLogger(new ConsoleLogger(), new WindowsEventLogger("RCube.Networking.Server"));
            _logger.Open();
            
            Services.AddService(typeof(Logger), _logger);
            Services.AddService(typeof(IServerComMgr), _comMgr);
            Services.AddService(typeof(IClientMgr), _clientMgr);
            Services.AddService(typeof(IMessageMgr), _msgMgr);

            _msgMgr.RegisterMessageHandler(typeof(RegisterClientMessage), new RegisterClientMsgHandler());
        }

        public override void Run()
        {
            Thread comThread = new Thread(_comMgr.Start);
            comThread.Start();
        }

        public override void Dispose()
        {
            _comMgr.Stop();
        }
    }
}
