using System;
using System.Collections.Generic;
using System.Linq;
using Common.Concrete.Command;
using Common.Concrete.Unit;
using Common.Enum;
using Common.Interface;
using UnityEngine;
using Common;

namespace ComponentFeature
{
    [RequireComponent(typeof(RequestEnter))]
    public class MCUDevice : IOTLinkDeviceEntity
    {
        public List<IOTDeviceEntity> IOTDevices
        {
            get
            {
                return GetFeature<LinkBehavior>().LinkedUnits
                    .Where(x => x.ToObject != null)
                    .Select(x => x.ToEntity as IOTDeviceEntity)
                    .ToList();
            }
        }
        [SerializeField] private RequestEnter RequestEnterPart;

        public event Action<IOTMessage> NotificationEvent;

        #region Unity 生命周期

        protected override void OnValidate()
        {
            base.OnValidate();
            RequestEnterPart = GetComponent<RequestEnter>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            RequestEnterPart.EnterRequestEvent += OnRequestEnter;
            NotificationEvent += OnNotification;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            RequestEnterPart.EnterRequestEvent -= OnRequestEnter;
            NotificationEvent -= OnNotification;
        }

        #endregion

        // 当收到外部请求时，生成IOT消息
        private void OnRequestEnter(EnterRequestUnit request)
        {
            if (request.RequestType == RequestType.Positive)
            {
                var msg = MsgGenerator.GenerateMsg(GetFeature<LinkBehavior>().LinkId, GetIOTDevice(request.DeviceLinkId), request);
                if (msg != null) MsgExchangePart.MsgSend(msg);
            }
        }

        // 当收到消息时，根据消息类型进行不同的处理
        protected override void OnReceiveMsg(ILinkingSendable sendable, IOTMessage msg)
        {
            if (msg.MsgType == IOTMsgType.Notification)
            {
                NotificationEvent?.Invoke(msg);
            }
        }
        // 当收到通知时，根据通知内容生成IOT消息
        private void OnNotification(IOTMessage msg)
        {
            var nrRequests = RequestEnterPart.GetNegativeUnits(msg.Parameter);
            foreach (var nrRequest in nrRequests)
            {
                MsgExchangePart.MsgSend(GenerateIOTMsg(nrRequest));
            }
        }

        #region MCU 行为方法
        /// <summary>
        /// 通过设置的请求生成IOT消息
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public IOTMessage GenerateIOTMsg(EnterRequestUnit unit)
        {
            if (unit.ParamType == typeof(IOTDeviceState))
            {
                return new IOTMessage(GetFeature<LinkBehavior>().LinkId, unit.DeviceLinkId, new TurnIOTDeviceCommand(GetIOTDevice(unit.DeviceLinkId), (IOTDeviceState)unit.Parameter), msgType: IOTMsgType.Command);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过 ID 获取设备
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IOTDeviceEntity GetIOTDevice(string id)
        {
            // 通过id获取设备
            return GetFeature<LinkBehavior>().LinkedUnits.Find(x => x.ToLinkBehavior.LinkId == id)?.ToEntity as IOTDeviceEntity;
        }

        [SerializeField] private Vector3 _gizmoSize = Vector3.one * 0.1f;
        private void OnDrawGizmos()
        {
            // 绘制 MCU 设备的黄色边界框
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _gizmoSize);
        }
        #endregion
    }
}

