using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace RC.Input.Events
{
    public delegate void mouseevent(Vector2 position, Vector2 move);


    public class MouseEvent:Event
    {
        Types.MouseInput m_type;
        Types.EventTypes m_eventtype;
        mouseevent m_mevent;

        MouseEvent m_me;
        
        public MouseEvent(Input.Types.MouseInput type, Input.Types.EventTypes eventtype, mouseevent mevent)
        {
            
            m_type = type;
            m_eventtype = eventtype;
            m_mevent = mevent;
            m_me = null;
        }

        public MouseEvent(Input.Types.MouseInput type, Input.Types.EventTypes eventtype, MouseEvent me)
        {
            m_type = type;
            m_eventtype = eventtype;
            m_mevent = null;
            m_me = me;

        }

        public MouseEvent execute(Vector2 position, Vector2 move)
        {
            if (m_mevent != null)
                m_mevent(position, move);

            return m_me;

        }

        public Types.MouseInput getType()
        {
            return m_type;
        }

        public Types.EventTypes getEvent()
        {
            return m_eventtype;
        }
    }
}
