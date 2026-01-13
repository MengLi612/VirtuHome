namespace Common.Enum
{
    public enum IOTMsgType
    {
        // 未设置具体类型
        Unknown = 0,
        // 命令类型，用于发送指令，通常在相应后直接生效
        Command = 1,
        // 响应类型，用于响应指令
        Response = 2,
        // 事件类型，用于发送事件，通常在相应后再特定条件下生效
        Event = 3,
        // 通知类型，用于发送通知，通常用于告知信息
        Notification = 4,
        // 请求类型，用于发送请求
        Request = 5,
        // 确认类型，用于确认消息，通常会返回确认消息
        Acknowledge = 6,
    }
}
