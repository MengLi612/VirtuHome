using System;
using Common.Abstract;
using Common.Concrete.Manager;
using UnityEngine;

namespace ComponentFeature
{
    public class Movement : AbstractIOTDeviceFeature
    {
        public enum MovementType
        {
            Stop,
            Move,
            Reverse
        }
        public MovementType CurStateId { get => _fsm.CurState; set => _fsm.CurState = value;}
        private readonly FSM<Movement, MovementType> _fsm = new();
        [field: SerializeField] public float Speed { get; set; } = 0.1f;
        public Vector3 Dir => transform.forward;
        [field: SerializeField] public bool IsReversed { get; set; } = false;

        [field: SerializeField] public bool UseRigidbody { get; set; } = false;
        [SerializeField] private Transform _trans;
        [SerializeField] private Rigidbody _rigidbody;

        public Action OnMove;

        private void Start()
        {
            _fsm.AddState(this, MovementType.Stop).EnterEvent += Stop;
            _fsm.AddState(this, MovementType.Move).FixedUpdateEvent += Move;
            _fsm.AddState(this, MovementType.Reverse).EnterEvent += Reverse;
            _fsm.StartState(MovementType.Stop);
        }

        private void FixedUpdate()
        {
            _fsm.FixedUpdate();
        }

        public void Move()
        {
            if (UseRigidbody)
            {
                _rigidbody.linearVelocity = (IsReversed ? -1 : 1) * Speed * Dir;
            }
            else
            {
                _trans.position += (IsReversed ? -1 : 1) * Speed * Time.deltaTime * Dir;
            }
            OnMove?.Invoke();
        }
        public void Reverse()
        {
            IsReversed = !IsReversed;
        }
        public void Stop()
        {
            if (UseRigidbody)
            {
                _rigidbody.linearVelocity = Vector3.zero;
            }
            else
            {
                _trans.position += Vector3.zero;
            }
        }

        #region 绘制 Gizmos
        [SerializeField] private Vector3 _startCube = Vector3.one;
        [SerializeField] private Vector3 _endCube = Vector3.one * 0.5f;
        [SerializeField] private float _lineLength = 2f;
        private void OnDrawGizmos()
        {
            // 绘制前进方向，在绘制的起点绘制一个box，终点位置绘制一个box
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position, _startCube);
            Gizmos.DrawRay(transform.position, Dir * _lineLength);
            Gizmos.DrawWireCube(transform.position + Dir * _lineLength, _endCube);
        }
        #endregion
    }
}
