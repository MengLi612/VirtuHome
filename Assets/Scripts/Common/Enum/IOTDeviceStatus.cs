using System;

namespace Common.Enum
{
    public enum LinkType
    {
        Wifi = 0,               // Wi-Fi
        Bluetooth = 1,          // 蓝牙
        USB = 2,                // USB
    }
    public enum LinkStateType
    {
        // 连接状态
        Disconnected = 0,       // 未连接
        Connecting = 1,         // 连接中
        Connected = 2           // 已连接
    }
    public enum DeviceStateType
    {
        // 设备状态
        Normal = 0,             // 正常
        Warning = 1,            // 警告
        Error = 2,              // 错误
        Critical = 3            // 严重错误
    }
    public enum OperatingStateType
    {
        // 运行状态
        Disable = 1 << 0,       // 禁用
        Active = 1 << 1,        // 活跃/工作中
        Standby = 1 << 2,       // 待机
        Sleeping = 1 << 3,      // 休眠
    }
}
