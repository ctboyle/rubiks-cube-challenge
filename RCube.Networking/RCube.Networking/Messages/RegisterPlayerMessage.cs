using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class RegisterPlayerMessage : DataMessage
    {
        public enum RegistrationType
        {
            Register,
            Unregister
        }

        private RegistrationType m_RegType;

        public RegisterPlayerMessage(DataMessageHeader header)
            : base(header)
        {
        }

        public RegistrationType Type
        {
            get { return m_RegType; }
            set { m_RegType = value; }
        }
    }
}
