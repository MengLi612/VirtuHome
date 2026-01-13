using System;
using System.Collections.Generic;
using Common.Concrete.Command;
using Common.Concrete.Unit;
using Common.Enum;
using Common.Interface;
using ComponentFeature;

namespace Common
{
    public static class MsgGenerator
    {
        public static List<Type> RecordTypes = new()
        {
            typeof(TimePoint),
        };

        public static IOTMessage GenerateMsg(Type paramType, string senderId, string receiverId, IOTDeviceEntity receiver, object msg)
        {
            // TODO: 后面需要持续维护可能的类型
            if (RecordTypes.Contains(paramType) && receiver is IRecordable<TimePoint> recordable && msg is TimePoint timePoint)
                return new IOTMessage(senderId, receiverId, new RecordCommand<TimePoint>(recordable, timePoint), IOTMsgType.Event);
            else if (paramType == typeof(IOTDeviceState))
                return new IOTMessage(senderId, receiverId, new TurnIOTDeviceCommand(receiver, msg as IOTDeviceState), IOTMsgType.Command);
            else
                return null;
        }
        public static IOTMessage GenerateMsg(string senderId, IOTDeviceEntity receiver, EnterRequestUnit unit)
        {
            return GenerateMsg(unit.ParamType, senderId, unit.DeviceLinkId, receiver, unit.Parameter);
        }
    }
}
