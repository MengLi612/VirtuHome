using System;
using System.Threading.Tasks;
using Common.Interface.Base;
using Core;

namespace Common.Abstract
{
    /// <summary>
    /// 在命令需要持续执行时使用AsyncCommand
    /// </summary>
    public abstract class AbstractAsyncCommand : IDisposable, IBaseCommand
    {
        public AbstractAsyncCommand()
        {
            id = Guid.NewGuid().ToString();
            ExecuteCompleted += OnExecuteCompleted;
        }

        private void OnExecuteCompleted(IBaseEventArgs args)
        {
            EventBus.Instance.Trigger(Id, null, args);
        }

        private readonly string id;
        private TaskCompletionSource<bool> _tcs = new();
        private bool _isRunning = false;
        private bool _isCompleted = false;
        private bool _isDisposed = false;

        public string Id => id;
        public bool IsRunning
        {
            get { return _isRunning; }
            protected set { _isRunning = value; }
        }
        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                _isCompleted = value;
                if (_isCompleted)
                {
                    _tcs.SetResult(true);
                }
            }
        }

        public event Action<IBaseEventArgs> ExecuteCompleted;


        public abstract bool CanExecute();
        public abstract void Undo();
        public virtual void Dispose()
        {
            if (_isDisposed) return;
            _isRunning = false;
            _isCompleted = true;
            _tcs.TrySetCanceled();
            _tcs = null;
            _isDisposed = true;
        }

        public virtual async Task ExecuteAsync(Action onComplete)
        {
            if (_isDisposed)
                throw new ObjectDisposedException(GetType().Name);

            IsRunning = true;
            IsCompleted = false;
            try
            {
                await ExecuteCoreAsync();
                onComplete?.Invoke();
                ExecuteCompleted?.Invoke(null);
            }
            finally
            {
                IsRunning = false;
            }
        }

        protected virtual Task ExecuteCoreAsync()
        {
            return WaitForFlagAsync();
        }

        protected Task WaitForFlagAsync()
        {
            if (IsCompleted)
                return Task.CompletedTask;

            return _tcs.Task;
        }

        protected void SetCompleted()
        {
            IsCompleted = true;
        }

    }
}
