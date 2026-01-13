using System;
using UnityEngine;

namespace Common.Concrete.Unit
{
    public record TimePoint
    {
        [Range(0, 23)]
        public int Hour;
        [Range(0, 59)]
        public int Minute;
        [Range(0, 59)]
        public int Second;
        /// <summary>
        /// 将所有时间单位转换为秒的时间点
        /// </summary>
        public int Time => Hour * 3600 + Minute * 60 + Second;
        public TimePoint(int hour, int minute, int second)
        {
            Hour = hour;
            Minute = minute;
            Second = second;
        }
    }
}
