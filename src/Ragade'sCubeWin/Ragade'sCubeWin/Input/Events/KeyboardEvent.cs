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
        
        public KeyboardEvent(Keys key, Types.EventTypes eventtype, keyboardevent kbevent)
        {
            m_key = key;
            m_eventtype = eventtype;
            m_kbevent = kbevent;
        }

        public override void execute(Keys key, Types.EventTypes eventtype)
        {
            if(key == m_key && eventtype == m_eventtype)
                m_kbevent();
        }

        public Keys getKey()
        {
            return m_key;
        }

        public Types.EventTypes getEvent()
        {
            return m_eventtype;
        }
    }
}
