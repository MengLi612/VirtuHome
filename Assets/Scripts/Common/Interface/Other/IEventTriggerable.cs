using System;
using Common.Enum;

namespace Common.Interface.Behavior
{
    public interface IEventTriggerable
    {
        /// <summary>
        ///     Trigger event
        /// </summary>
        /// <param name="eventType">Event type</param>
        void TriggerEvent<T>(Action<T> action, T args);
    }
}
