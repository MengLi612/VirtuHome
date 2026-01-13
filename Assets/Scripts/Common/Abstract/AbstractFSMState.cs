using Common.Concrete.Manager;
using Common.Interface;

namespace Common.Abstract
{
    public abstract class AbstractFSMState<TOwner, TType> : IState
    {
        public AbstractFSMState(TOwner owner, FSM<TOwner, TType> stateMachine)
        {
            Owner = owner;
            StateMachine = stateMachine;
        }
        protected TOwner Owner { get; }
        protected FSM<TOwner, TType> StateMachine { get; set; }

        public virtual void OnEnter() { }
        public virtual void OnExit() { }
        public virtual void Update() { }
        public virtual void FixedUpdate() { }
        protected virtual void ChangeState(TType state) => StateMachine.ChangeState(state);

    }
}
