using System;
using Common.Interface.Base;

namespace Common.Abstract
{
    public abstract class AbstractCommand : IDisposable, IBaseCommand
    {
        public abstract bool CanExecute();
        public abstract void Execute();
        public virtual void Undo() { }
        public virtual void Dispose() { }
    }
}
