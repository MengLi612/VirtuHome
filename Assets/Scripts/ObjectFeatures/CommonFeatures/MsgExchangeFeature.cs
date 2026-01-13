using System;
using System.Collections.Generic;
using Common.Abstract;
using Common.Concrete.Unit;
using Common.Enum;
using Common.Interface;
using UnityEngine;

namespace ComponentFeature
{
    [RequireComponent(typeof(LinkBehavior))]
    public class MsgExchangeFeature : AbstractIOTDeviceFeature, ILinkingSendable, ILinkingReceivable
    {
        [SerializeField] private LinkBehavior _linkBehavior;
        private readonly List<ILinkingReceivable> _receivables = new();

        public List<ILinkingReceivable> Receivables
        {
            get
            {
                if (_linkBehavior != null && _linkBehavior.CurState == LinkStateType.Connected)
                {
                    foreach (var item in _linkBehavior.LinkedUnits)
                    {
                        if (item.ToObject != null)
                        {
                            var receivable = item.ToEntity.GetComponent<ILinkingReceivable>();
                            if (receivable != null && !_receivables.Contains(receivable))
                                _receivables.Add(receivable);
                        }
                    }
                }
                return _receivables;
            }
        }

        private readonly List<ILinkingSendable> _sendables = new();

        public event Action<ILinkingSendable, IOTMessage> MsgReceived;

        public List<ILinkingSendable> Sendables
        {
            get
            {
                if (_linkBehavior != null && _linkBehavior.CurState == LinkStateType.Connected)
                {
                    foreach (var item in _linkBehavior.LinkedUnits)
                    {
                        if (item.ToObject != null)
                        {
                            var sendable = item.ToEntity.GetComponent<ILinkingSendable>();
                            if (sendable != null && !_sendables.Contains(sendable))
                                _sendables.Add(sendable);
                        }
                    }
                }
                return _sendables;
            }
        }


        private void OnValidate()
        {
            _linkBehavior = GetComponent<LinkBehavior>();
        }
        public void MsgSend(IOTMessage msg)
        {
            if (msg.ReceiverId == null) return;
            LinkUnit receiverLink = _linkBehavior.GetLinkUnit(msg.ReceiverId);
            ILinkingReceivable receivable = receiverLink?.ToEntity.GetComponent<ILinkingReceivable>();

            if (receivable == null)
            {
                Debug.LogError($"IOTDevice Link: {msg.SenderId} -> {msg.ReceiverId} is not found");
            }
            else
            {
                Debug.Log($"IOTDevice Link: {msg.SenderId} -> {msg.ReceiverId}");
                receivable.MsgReceive(this, msg);
            }
        }
        public void MsgSend(object param, IOTMsgType msgType)
        {

        }
        public void GenerateMsg(IOTMsgType msgType, object param)
        {

        }

        public void MsgReceive(ILinkingSendable sendable, IOTMessage msg)
        {
            Debug.Log($"IOTDevice Link: {msg.ReceiverId} <- {msg.SenderId}");
            MsgReceived?.Invoke(sendable, msg);
        }
    }
}
