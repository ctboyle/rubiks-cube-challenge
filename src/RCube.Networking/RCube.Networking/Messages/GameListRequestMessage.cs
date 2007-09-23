using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class GameListRequestMessage : DataMessage
    {
        public GameListRequestMessage(DataMessageHeader header)
            : base(header)
        {
        }
    }
}
