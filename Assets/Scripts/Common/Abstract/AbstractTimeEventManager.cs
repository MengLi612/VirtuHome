using System.Collections.Generic;
using ComponentFeature;
using Core;
using UnityEngine;

namespace Common.Abstract
{
    public abstract class AbstractTimeEventManager : ComponentBehavior
    {
        public abstract class TimeEvent
        {
            public enum TimeEventType
            {
                None,
                Transform,
                LinkOthers,
            }
            public float RecordTime { get; set; }
            public TimeEventType EventType { get; set; }
        }

        public abstract Dictionary<IOTDeviceEntity, TimeEvent> IOTTimeEventDic { get; }
        [SerializeField] protected GlobalTimer _timer;

        public abstract void AddTimeEvent(IOTDeviceEntity entity, TimeEvent timeEvent);
        public abstract void RemoveTimeEvent(IOTDeviceEntity entity, TimeEvent timeEvent);
        public abstract void UpdateDeviceState();
    }
}
