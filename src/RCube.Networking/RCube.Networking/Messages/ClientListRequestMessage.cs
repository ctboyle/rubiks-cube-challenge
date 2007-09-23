using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class ClientListRequestMessage : DataMessage
    {
        public ClientListRequestMessage(DataMessageHeader header)
            : base(header)
        {
        }
    }
}
