using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input.Events
{
    public delegate void keyboardevent();

    public class KeyboardEvent:Event
    {
        Keys m_key;
        Types.EventTypes m_eventtype;
        keyboardevent m_kbevent;
        KeyboardEvent m_kbe;
        
        public KeyboardEvent(Keys key, Types.EventTypes eventtype, keyboardevent kbevent)
        {
            m_key = key;
            m_eventtype = eventtype;
            m_kbevent = kbevent;
            m_kbe = null;
        }

        public KeyboardEvent(Keys key, Types.EventTypes eventtype, KeyboardEvent kbe)
        {
            m_key = key;
            m_eventtype = eventtype;
            m_kbevent = null;
            m_kbe = kbe;
        }

        public KeyboardEvent execute()
        {
            if(m_kbevent != null)
            {
                m_kbevent();
                return null;
            }
            else
            {
                return m_kbe;
            }
        }

        public Keys getKey()
        {
            return m_key;
        }

        public Types.EventTypes getEventType()
        {
            return m_eventtype;
        }

        public Event getEvent()
        {
            if (m_kbe != null)
                return m_kbe;
            else
                throw new Exception();
        }
    }
}
