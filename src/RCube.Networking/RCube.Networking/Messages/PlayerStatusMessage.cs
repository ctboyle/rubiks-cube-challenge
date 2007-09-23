using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class PlayerStatusMessage : DataMessage
    {
        public PlayerStatusMessage(DataMessageHeader header)
            : base(header)
        {
        }
    }
}
