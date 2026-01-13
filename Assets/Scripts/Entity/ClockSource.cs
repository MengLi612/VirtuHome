using System.Collections.Generic;
using System.Linq;
using Common.Concrete.Unit;
using Common.Enum;
using Common.Interface;
using UnityEngine;

namespace ComponentFeature
{
    [RequireComponent(typeof(Timer))]
    public class ClockSource : IOTLinkDeviceEntity, IRecordable<TimePoint>
    {
        [SerializeField] private Timer Timer;
        [SerializeField] private string mcuId;

        [field: SerializeField] public List<TimePoint> Records { get; set; } = new();

        protected override void OnValidate()
        {
            base.OnValidate();
            if (Timer == null) Timer = GetComponent<Timer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Timer.ClockTimeChanged += OnTimeChanged;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            Timer.ClockTimeChanged -= OnTimeChanged;
        }

        private void OnTimeChanged(TimePoint point)
        {
            foreach (var record in Records)
            {
                if (record.Time == point.Time)
                {
                    OnRecordReached(record);
                }
            }
        }

        private void OnRecordReached(TimePoint record)
        {
            MsgExchangePart.MsgSend(new IOTMessage(LinkBehavior.LinkId, mcuId, record, IOTMsgType.Notification));
        }

        public IEnumerable<string> GetIOTDeviceLinkIds()
        {
            // 获取所有设备的id
            return LinkBehavior.LinkedUnits.Select(x => x.ToLinkBehavior.LinkId);
        }
    }
}
