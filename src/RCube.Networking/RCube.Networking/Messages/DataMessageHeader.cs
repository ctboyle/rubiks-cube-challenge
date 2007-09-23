using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace RCube.Networking.Messages
{
    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct DataMessageHeader
    {
        public Guid SessionId;
        public Guid GameId;
        public Guid MessageId;
        public DateTime Timestamp;
    }
}
