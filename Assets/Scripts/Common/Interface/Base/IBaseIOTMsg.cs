using System;
using Common.Enum;

namespace Common.Interface.Base
{
    public interface IBaseIOTMsg
    {
        string SenderId { get; }
        string ReceiverId { get; }
        IOTMsgType MsgType { get; }
        Type ParamType { get; }
        public object Parameter { get; }
    }
}
