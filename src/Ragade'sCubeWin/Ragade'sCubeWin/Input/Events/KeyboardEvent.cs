using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;

namespace RagadesCubeWin.Input.Events
{
    public delegate void keyboardevent();
    public delegate void keyboardevent2(Keys key);

    public class KeyboardEvent:Event
    {
        Keys m_key;
        Types.EventTypes m_eventtype;
        keyboardevent m_kbevent;
        keyboardevent2 m_kbevent2;
        KeyboardEvent m_kbe;
        bool all;

        
        public KeyboardEvent(Keys key, Types.EventTypes eventtype, keyboardevent kbevent)
        {
            m_key = key;
            m_eventtype = eventtype;
            m_kbevent = kbevent;
            m_kbevent2 = null;
            m_kbe = null;
            all = false;
        }

        public KeyboardEvent(Keys key, Types.EventTypes eventtype, KeyboardEvent kbe)
        {
            m_key = key;
            m_eventtype = eventtype;
            m_kbevent = null;
            m_kbevent2 = null;
            m_kbe = kbe;
            all = false;
        }

        public KeyboardEvent(Types.EventTypes eventtype, keyboardevent2 kbe)
        {
            m_eventtype = eventtype;
            m_kbevent = null;
            m_kbevent2 = kbe;
            m_kbe = null;
            all = true;
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

        public void execute(Keys k)
        {
            m_kbevent2(k);
        }


        public bool ALL
        {
            get
            {
                return all;
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
