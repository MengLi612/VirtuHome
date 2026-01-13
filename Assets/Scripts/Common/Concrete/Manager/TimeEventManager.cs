using System.Collections.Generic;
using Common.Abstract;
using ComponentFeature;

namespace Common.Concrete.Manager
{
    public class TimeEventManager : AbstractTimeEventManager
    {
        public override Dictionary<IOTDeviceEntity, TimeEvent> IOTTimeEventDic { get; } = new();

        public override void AddTimeEvent(IOTDeviceEntity entity, TimeEvent timeEvent) => IOTTimeEventDic.Add(entity, timeEvent);

        public override void RemoveTimeEvent(IOTDeviceEntity entity, TimeEvent timeEvent) => IOTTimeEventDic.Remove(entity);

        public override void UpdateDeviceState()
        {
            throw new System.NotImplementedException();
        }

    }
}