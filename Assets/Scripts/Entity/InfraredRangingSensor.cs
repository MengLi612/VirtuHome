using System.Collections.Generic;
using Common.Interface;
using UnityEngine;

namespace ComponentFeature
{

    [RequireComponent(typeof(InfraredRanging))]
    public class InfraredRangingSensor : IOTLinkDeviceEntity, IRecordable<float>
    {
        [field: SerializeField] public InfraredRanging InfraredRangingPart { get; private set; }

        [field: SerializeField] public List<float> Records { get; set; } = new();
        protected override void OnValidate()
        {
            base.OnValidate();
            if (InfraredRangingPart == null) InfraredRangingPart = GetComponent<InfraredRanging>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            InfraredRangingPart.DistanceChanged += OnDistanceChanged;
        }
        protected override void OnDisable()
        {
            base.OnDisable();
            InfraredRangingPart.DistanceChanged -= OnDistanceChanged;
        }

        private void OnDistanceChanged(float obj)
        {
            foreach (var record in Records)
            {
                //TODO: 可以在这里添加处理逻辑，例如触发事件或记录日志
                if (record - obj < 0.01f)
                {
                }
            }
        }
    }
}
