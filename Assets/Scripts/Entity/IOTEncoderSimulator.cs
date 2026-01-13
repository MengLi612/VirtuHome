using Common.Concrete.Unit;
using Common.Interface;
using UnityEngine;

namespace ComponentFeature
{
    [RequireComponent(typeof(Movement))]
    public class IOTEncoderSimulator : IOTLinkDeviceEntity
    {
        [field: SerializeField] public Movement MovementPart { get; private set; }


        protected override void OnValidate()
        {
            base.OnValidate();
            MovementPart = GetComponent<Movement>();
        }
        protected override void OnEnable()
        {
            base.OnEnable();
        }
        protected override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnReceiveMsg(ILinkingSendable sendable, IOTMessage msg)
        {
            base.OnReceiveMsg(sendable, msg);
        }
    }
}