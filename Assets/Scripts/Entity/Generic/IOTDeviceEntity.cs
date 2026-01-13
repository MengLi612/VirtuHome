using System.Collections.Generic;
using System.Linq;
using Common.Abstract;
using Common.Concrete.Manager;
using Common.Enum;
using Common.Interface;
using Core;
using UnityEngine;

namespace ComponentFeature
{

    public abstract class IOTDeviceEntity : Entity
    {
        [field: SerializeField] public List<AbstractIOTDeviceFeature> Features { get; set; } = new();

        #region Unity 生命周期
        protected virtual void OnValidate()
        {
            Features = GetComponentsInChildren<AbstractIOTDeviceFeature>().ToList();
        }
        protected virtual void Start()
        {
            _stateMachine.States.Add(OperatingStateType.Disable, new DisableState(this, _stateMachine));
            _stateMachine.States.Add(OperatingStateType.Active, new ActiveState(this, _stateMachine));
            _stateMachine.StartState(OperatingStateType.Disable);
        }
        protected virtual void Update()
        {
            _stateMachine.Update();
        }
        #endregion

        #region 设备状态相关
        private readonly FSM<IOTDeviceEntity, OperatingStateType> _stateMachine = new();
        protected virtual IStateable<OperatingStateType> StateMachine => _stateMachine;
        public OperatingStateType CurrentState
        {
            get => StateMachine.CurState;
            set
            {
                if (StateMachine.CurState == value) return;
                Debug.Log($"Device {name} state changed to {value}");
                StateMachine.CurState = value;
            }
        }
        #endregion

        public T GetFeature<T>() where T : AbstractIOTDeviceFeature
        {
            foreach (var feature in Features)
            {
                if (feature is T t)
                {
                    return t;
                }
            }
            return default;
        }
        public T GetBehavior<T>()
        {
            return GetComponentInChildren<T>();
        }


        private class DisableState : AbstractFSMState<IOTDeviceEntity, OperatingStateType>
        {
            public DisableState(IOTDeviceEntity owner, FSM<IOTDeviceEntity, OperatingStateType> stateMachine) : base(owner, stateMachine) { }

            public override void OnEnter()
            {
                Owner.Features.ForEach(f => f.enabled = false);
                Owner.enabled = false;
            }
        }
        private class ActiveState : AbstractFSMState<IOTDeviceEntity, OperatingStateType>
        {
            public ActiveState(IOTDeviceEntity owner, FSM<IOTDeviceEntity, OperatingStateType> stateMachine) : base(owner, stateMachine) { }

            public override void OnEnter()
            {
                Owner.enabled = true;
                Owner.Features.ForEach(f => f.enabled = true);
            }
        }
    }
}

