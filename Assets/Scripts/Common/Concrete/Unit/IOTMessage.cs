using System;
using Common.Enum;
using Common.Interface.Base;

namespace Common.Concrete.Unit
{
    public class IOTMessage : IBaseIOTMsg
    {
        public string SenderId { get; }
        public string ReceiverId { get; }
        public IOTMsgType MsgType { get; }
        public object Parameter { get; }
        public Type ParamType => Parameter.GetType();

        public IOTMessage(string senderId, string receiverId, object message, IOTMsgType msgType)
        {
            SenderId = senderId;
            ReceiverId = receiverId;
            Parameter = message;
            MsgType = msgType;
        }
    }
}