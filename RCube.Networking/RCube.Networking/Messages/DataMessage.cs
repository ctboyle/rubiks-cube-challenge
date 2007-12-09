using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public abstract class DataMessage
    {
        private DataMessageHeader m_Header;
        private bool m_Acknowledged;

        public DataMessage (DataMessageHeader header)
        {
            m_Header = header;
        }

        public DataMessageHeader Header
        {
            get { return m_Header; }
        }

        public bool NeedsAcknowledgement
        {
            get { return m_Acknowledged; }
            set { m_Acknowledged = value; }
        }
    }
}
