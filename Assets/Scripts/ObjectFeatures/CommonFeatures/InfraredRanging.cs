using System;
using Common.Abstract;
using RaycastPro.RaySensors;
using UnityEngine;
namespace ComponentFeature
{
    public class InfraredRanging : AbstractIOTDeviceFeature
    {
        [field: SerializeField] public BasicRay Ray { get; private set; }
        public float RayLength 
        { 
            get => Ray.direction.z;
            set => Ray.direction.Set(Ray.direction.x, Ray.direction.y, value);
        }
        private bool _preIsHit = false;
        private float _preDistance = -1f;
        public bool IsHit => Ray.Performed;
        public float Distance => IsHit ? Ray.HitDistance : -1f;

        public Action<bool> HitChanged;
        public Action<float> DistanceChanged;

        private void OnValidate()
        {
            if (Ray == null) Ray = GetComponentInChildren<BasicRay>();
        }
        public void OnEnable()
        {
            Ray.enabled = true;
        }
        public void OnDisable()
        {
            Ray.enabled = false;
        }
        private void Update()
        {
            if (IsHit != _preIsHit)
            {
                HitChanged?.Invoke(IsHit);
                _preIsHit = IsHit;
            }
            if (Distance != _preDistance)
            {
                DistanceChanged?.Invoke(Distance);
                _preDistance = Distance;
            }
        }
    }
}
