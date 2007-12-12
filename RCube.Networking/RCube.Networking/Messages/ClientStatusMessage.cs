using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class ClientStatusMessage : DataMessage
    {
        public ClientStatusMessage(DataMessageHeader header)
            : base(header)
        {
        }
    }
}
