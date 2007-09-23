using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace RCube.Networking.Messages
{
    [Serializable]
    public class RegisterClientMessage : DataMessage
    {
        public enum RegistrationType
        {
            Register,
            Unregister
        }

        private RegistrationType m_RegistrationType;

        public RegisterClientMessage(DataMessageHeader header) 
            : base(header)
        {
            m_RegistrationType = RegistrationType.Register;
        }

        public RegistrationType Type
        {
            get { return m_RegistrationType; }
            set { m_RegistrationType = value; }
        }
    }
}
