using Common.Interface.Base;
using Core;
using UnityEngine;
using static Common.Constants.EventKeys;

namespace ComponentFeature
{
    public record CollisionEventArgs : IBaseEventArgs
    {
        private readonly GameObject self;
        private readonly Collision otherCollision;
        public CollisionEventArgs(GameObject self, Collision other)
        {
            otherCollision = other;
            this.self = self;
        }
        public Collision Other => otherCollision;
        public GameObject Self => self;
    }
    public record TriggerEventArgs : IBaseEventArgs
    {
        private readonly GameObject self;
        private readonly Collider otherCollider;
        public TriggerEventArgs(GameObject self, Collider other)
        {
            otherCollider = other;
            this.self = self;
        }
        public Collider Other => otherCollider;
        public GameObject Self => self;
    }

    [RequireComponent(typeof(Collider))]
    public class ColliderHandler : ControllerBehavior
    {
        private void OnCollisionEnter(Collision other)
        {
            Debug.Log($"Collision Enter detected on {gameObject.name} with {other.gameObject.name}");
            EventPart.Trigger(Lifecycle.OnCollisionEnter + gameObject.GetInstanceID(), other.gameObject.GetComponentInParent<Entity>(), new CollisionEventArgs(gameObject, other));
        }
        private void OnCollisionExit(Collision other)
        {
            EventPart.Trigger(Lifecycle.OnCollisionExit + gameObject.GetInstanceID(), other.gameObject.GetComponentInParent<Entity>(), new CollisionEventArgs(gameObject, other));
        }
        private void OnCollisionStay(Collision other)
        {
            EventPart.Trigger(Lifecycle.OnCollisionStay + gameObject.GetInstanceID(), other.gameObject.GetComponentInParent<Entity>(), new CollisionEventArgs(gameObject, other));
        }
        private void OnTriggerEnter(Collider other)
        {
            EventPart.Trigger(Lifecycle.OnTriggerEnter + gameObject.GetInstanceID(), other.gameObject.GetComponentInParent<Entity>(), new TriggerEventArgs(gameObject, other));
        }
        private void OnTriggerExit(Collider other)
        {
            EventPart.Trigger(Lifecycle.OnTriggerExit + gameObject.GetInstanceID(), other.gameObject.GetComponentInParent<Entity>(), new TriggerEventArgs(gameObject, other));
        }
        private void OnTriggerStay(Collider other)
        {
            EventPart.Trigger(Lifecycle.OnTriggerStay + gameObject.GetInstanceID(), other.gameObject.GetComponentInParent<Entity>(), new TriggerEventArgs(gameObject, other));
        }
    }
}
