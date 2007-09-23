using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class PlayerListRequestMessage : DataMessage
    {
        public PlayerListRequestMessage(DataMessageHeader header)
            : base(header)
        {
        }
    }
}
