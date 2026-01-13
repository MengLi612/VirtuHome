using Common.Abstract;
using Common.Concrete.Unit;
using Common.Enum;
using Common.Interface;
using UnityEngine;

namespace ComponentFeature
{
    /// <summary>
    /// 继承自IOTDeviceEntity，并添加了连接和消息处理相关的组件。
    /// </summary>
    [RequireComponent(typeof(MsgExchangeFeature))]
    [RequireComponent(typeof(LinkBehavior))]
    public class IOTLinkDeviceEntity : IOTDeviceEntity
    {
        [field: SerializeField] public LinkBehavior LinkBehavior { get; private set; }
        [field: SerializeField] public MsgExchangeFeature MsgExchangePart { get; private set; }

        protected override void OnValidate()
        {
            base.OnValidate();
            if (LinkBehavior == null) LinkBehavior = GetComponent<LinkBehavior>();
            if (MsgExchangePart == null) MsgExchangePart = GetComponent<MsgExchangeFeature>();
        }
        protected virtual void OnEnable() => MsgExchangePart.MsgReceived += OnReceiveMsg;
        protected virtual void OnDisable() => MsgExchangePart.MsgReceived -= OnReceiveMsg;
        protected virtual void OnReceiveMsg(ILinkingSendable sendable, IOTMessage msg)
        {
            if (msg.MsgType == IOTMsgType.Command || msg.MsgType == IOTMsgType.Event ||msg.MsgType == IOTMsgType.Unknown)
            {
                if (msg.Parameter is AbstractCommand command)   
                {
                    if (command.CanExecute())
                    {
                        command.Execute();
                    }
                }
            }
        }
    }
}


