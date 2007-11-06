using System;
using System.Collections.Generic;
using RC.Input.Events;
using RC.Engine.Input;

namespace RC.Input.Watchers
{
    public abstract class RCWatcher<EventType> : IWatcher, IEnumerable<EventType> where EventType : Event
    {
        private List<EventType> _events;

        public RCWatcher()
        {
            _events = new List<EventType>();
        }

        public bool WatchEvent(EventType e)
        {
            try
            {
                if (_events.Contains(e))
                    return false;
                else
                {
                    _events.Add(e);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public void WatchEvents(IEnumerable<EventType> events)
        {
            foreach (EventType e in events)
                WatchEvent(e);
        }

        public bool RemoveEvent(EventType e)
        {
            try
            {
                if (!_events.Contains(e))
                    return false;
                else
                {
                    _events.Remove(e);
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public IEnumerator<EventType> GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _events.GetEnumerator();
        }

        public abstract bool DetectMyInput();
        public abstract void RunEvents();
    }
}