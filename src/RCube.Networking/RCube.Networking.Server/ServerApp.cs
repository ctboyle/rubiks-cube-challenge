using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;

namespace RCube.Networking.Server
{
    public abstract class ServerApp : IDisposable
    {
        ServiceContainer _svcContainer;

        public ServerApp()
        {
            _svcContainer = new ServiceContainer();
            Initialize();
        }

        public ServiceContainer Services
        {
            get { return _svcContainer; }
        }

        protected abstract void Initialize();
        public abstract void Run();
        public abstract void Dispose();
    }
}
