using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;


namespace RagadesCubeWin.Input.Events
{
    public delegate void geventanalog(Vector2 position, Vector2 move);
    public delegate void geventbinary();

    class XBox360GamePadEvent:Event
    {
        #region vars
        Types.XBox360GamePadTypes m_type;
        Types.EventTypes m_eventtype;
        geventanalog m_geventa;
        geventbinary m_geventb;

        XBox360GamePadEvent m_ge;
        #endregion

        public XBox360GamePadEvent(Input.Types.XBox360GamePadTypes type, Input.Types.EventTypes eventtype, geventanalog gevent)
        {
            m_type = type;
            m_eventtype = eventtype;
            m_geventa = gevent;
            m_geventb = null;
            m_ge = null;
        }

        public XBox360GamePadEvent(Input.Types.XBox360GamePadTypes type, Input.Types.EventTypes eventtype, geventbinary gevent)
        {

            m_type = type;
            m_eventtype = eventtype;
            m_geventb = gevent;
            m_geventa = null;
            m_ge = null;
        }

        public XBox360GamePadEvent(Input.Types.XBox360GamePadTypes type, Input.Types.EventTypes eventtype, XBox360GamePadEvent ge)
        {
            m_type = type;
            m_eventtype = eventtype;
            m_geventa = null;
            m_geventb = null;
            m_ge = ge;

        }

        public XBox360GamePadEvent execute(Vector2 position, Vector2 move)
        {
            if (m_geventa != null)
                m_geventa(position, move);

            return m_ge;

        }

        public XBox360GamePadEvent execute()
        {
            if (m_geventb != null)
                m_geventb();

            return m_ge;
        }

        public Types.XBox360GamePadTypes getType()
        {
            return m_type;
        }

        public Types.EventTypes getEvent()
        {
            return m_eventtype;
        }
    }
}
