using System;
using System.Threading.Tasks;
using Common.Interface.Base;
using Core;
using static Common.Constants.EventKeys;

namespace Common.Abstract
{
    /// <summary>
    /// 在命令需要持续执行时使用MonoCommand
    /// </summary>
    public abstract class AbstractMonoCommand : AbstractAsyncCommand
    {
        protected EventBus EventPart => EventBus.Instance;

        protected virtual void OnEnable(Entity entity, IBaseEventArgs args) { }
        protected virtual void OnDisable(Entity entity, IBaseEventArgs args) { }
        protected virtual void OnFixedUpdate(Entity entity, IBaseEventArgs args) { }
        protected virtual void OnUpdate(Entity entity, IBaseEventArgs args) { }

        public override void Dispose()
        {
            EventPart.UnRegister<IBaseEventArgs>(Lifecycle.Update, OnUpdate);
            EventPart.UnRegister<IBaseEventArgs>(Lifecycle.FixedUpdate, OnFixedUpdate);
        }

        public override async Task ExecuteAsync(Action onComplete)
        {
            EventPart.Register<IBaseEventArgs>(Lifecycle.OnEnable, OnEnable);
            EventPart.Register<IBaseEventArgs>(Lifecycle.OnDisable, OnDisable);
            EventPart.Register<IBaseEventArgs>(Lifecycle.Update, OnUpdate);
            EventPart.Register<IBaseEventArgs>(Lifecycle.FixedUpdate, OnFixedUpdate);
            await base.ExecuteAsync(onComplete);
        }
    }
}
