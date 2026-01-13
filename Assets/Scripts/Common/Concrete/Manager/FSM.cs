using System;
using System.Collections.Generic;
using Common.Abstract;
using Common.Concrete.Unit;
using Common.Interface;

namespace Common.Concrete.Manager
{
    public class FSMQuickState<TOwner, TType> : AbstractFSMState<TOwner, TType>, IDisposable
    {
        public FSMQuickState(TOwner owner, FSM<TOwner, TType> stateMachine) : base(owner, stateMachine) { }

        public Action EnterEvent { get; set; }
        public Action ExitEvent { get; set; }
        public Action UpdateEvent { get; set; }
        public Action FixedUpdateEvent { get; set; }

        public override void OnEnter() => EnterEvent?.Invoke();
        public override void OnExit() => ExitEvent?.Invoke();
        public override void Update() => UpdateEvent?.Invoke();
        public override void FixedUpdate() => FixedUpdateEvent?.Invoke();

        public void Dispose()
        {
            EnterEvent = null;
            ExitEvent = null;
            UpdateEvent = null;
            FixedUpdateEvent = null;
        }
    }


    public class FSM<TOwner, T> : IStateable<T>
    {
        public Type StateType => typeof(T);
        private T stateId;
        public T CurState { get => stateId; set => ChangeState(value); }
        public Dictionary<T, AbstractFSMState<TOwner, T>> States { get; set; } = new();

        public void StartState(T state)
        {
            if (States.ContainsKey(state))
            {
                stateId = state;
                States[CurState].OnEnter();
            }
        }
        public void ChangeState(T state)
        {
            if (States.ContainsKey(state) && !state.Equals(CurState))
            {
                States[CurState].OnExit();
                stateId = state;
                States[CurState].OnEnter();
            }
        }
        public void Update() => States[CurState].Update();
        public void FixedUpdate() => States[CurState].FixedUpdate();


        #region 快捷方法

        /// <summary>
        /// 快捷添加一个状态，并能够通过链式调用添加事件
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        public FSMQuickState<TOwner, T> AddState(TOwner owner, T stateId)
        {
            var state = new FSMQuickState<TOwner, T>(owner, this);
            States.Add(stateId, state);
            return state;
        }
        public void RemoveState(T stateId)
        {
            if (States.ContainsKey(stateId) && States[stateId] is IDisposable disposable)
            {
                disposable.Dispose();
            }

            States.Remove(stateId);
        }
        #endregion
    }
}
