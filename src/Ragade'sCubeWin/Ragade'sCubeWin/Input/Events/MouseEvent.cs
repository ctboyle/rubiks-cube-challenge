using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace RagadesCubeWin.Input.Events
{
    public delegate void mouseevent(Vector2 position, Vector2 move);


    class MouseEvent:Event
    {
        Types.MouseButtonTypes m_type;
        Types.EventTypes m_eventtype;
        mouseevent m_mevent;
        
        public MouseEvent(Input.Types.MouseButtonTypes type, Input.Types.EventTypes eventtype, mouseevent mevent)
        {
            
            m_type = type;
            m_eventtype = eventtype;
            m_mevent = mevent;
        }

        public void execute(Vector2 position, Vector2 move)
        {
            m_mevent(position, move);
        }

        public Types.MouseButtonTypes getType()
        {
            return m_type;
        }

        public Types.EventTypes getEvent()
        {
            return m_eventtype;
        }
    }
}
