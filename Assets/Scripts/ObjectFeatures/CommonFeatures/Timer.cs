using System;
using Common.Abstract;
using Common.Concrete.Unit;
using Common.Enum;
using Common.Interface.Behavior;
using UnityEngine;

namespace ComponentFeature
{
    public class Timer : AbstractIOTDeviceFeature, IEventTriggerable
    {
        [field: SerializeField]
        [field: Range(0, 86399.0f)]
        public float CurTime { get; set; } = 0;
        private float accumulatedTime = 0f; // 暂存累计时间（浮点数）
        public Action<TimePoint> ClockTimeChanged { get; set; }

        [Range(0, 23)]
        public int CurHour
        {
            get => (int)CurTime / 3600;
            set => SetCurTime(value, null, null);
        }
        [Range(0, 59)]
        public int CurMinute
        {
            get => (int)CurTime % 3600 / 60;
            set => SetCurTime(null, value, null);
        }
        [Range(0, 59)]
        public int CurSecond
        {
            get => (int)CurTime % 60;
            set => SetCurTime(null, null, value);
        }

        public string NowTimeShort => GlobalTimer.Instance.GetFormattedTime();

        private void Update()
        {
            // 累积时间
            accumulatedTime += Time.deltaTime;

            // 每过1秒，整数秒数加1
            if (accumulatedTime >= 1f)
            {
                accumulatedTime -= 1f; // 保留不足1秒的部分
                CurTime += 1;
                if (CurTime >= 86400)
                    CurTime = 0;
                TimePoint curTimePoint = new(CurHour, CurMinute, CurSecond);
                TriggerEvent(ClockTimeChanged, curTimePoint);
            }
        }

        public string GetFormattedCurTime(TimeFormat timeFormat)
        {
            return timeFormat switch
            {
                TimeFormat.HHmmss => string.Format("{0:D2}:{1:D2}:{2:D2}", CurHour, CurMinute, CurSecond),
                TimeFormat.HHmm => string.Format("{0:D2}:{1:D2}", CurHour, CurMinute),
                _ => string.Format("{0:D2}:{1:D2}:{2:D2}", CurHour, CurMinute, CurSecond),
            };
        }

        public void SetCurTime(int? hour = null, int? minute = null, int? second = null)
        {
            if (hour != null)
                CurTime = hour.Value * 3600 + CurTime % 3600;
            if (minute != null)
            {
                int tempHour = (int)CurTime / 3600;
                CurTime = tempHour * 3600 + minute.Value * 60 + CurTime % 60;
            }
            if (second != null)
                CurTime = second.Value + (int)CurTime / 60 * 60;
        }
        public void UpdateCurTimeToNow() => SetCurTimeToNow(TimeType.GlobalTime);
        public void SetCurTimeToNow(TimeType timeType)
        {
            if (timeType == TimeType.GlobalTime) CurTime = GlobalTimer.Instance.GlobalTime;
            else CurTime = DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
        }

        public void TriggerEvent<T>(Action<T> action, T arg)
        {
            action?.Invoke(arg);
        }
    }
}