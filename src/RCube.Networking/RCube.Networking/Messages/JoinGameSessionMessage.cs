using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class JoinGameSessionMessage : DataMessage
    {
        public enum JoinType
        {
            Join,
            Quit
        }

        private JoinType m_JoinType;

        public JoinGameSessionMessage(DataMessageHeader header)
            : base(header)
        {
        }

        public JoinType Type
        {
            get { return m_JoinType; }
            set { m_JoinType = value; }
        }
    }
}
