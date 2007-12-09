using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    public class PingClientMessage : DataMessage
    {
        public enum PingType
        {
            Ping,
            Pong
        }

        private PingType m_PingType;

        public PingClientMessage(DataMessageHeader header) 
            : base(header)
        {
            m_PingType = PingType.Ping;
        }

        public PingType ResponseType
        {
            get { return PingType.Ping; }
            set { }
        }

        public PingType Type
        {
            get { return m_PingType; }
            set { m_PingType = value; }
        }
    }
}
