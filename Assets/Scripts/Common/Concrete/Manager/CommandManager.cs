using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Abstract;
using Common.Enum;
using Common.Interface;
using UnityEngine;

namespace Common.Concrete.Manager
{
    public class AsyncCommandManager : IDisposable, ISingletonable<AsyncCommandManager>
    {
        private static AsyncCommandManager _instance;
        public static AsyncCommandManager Instance => _instance ??= new AsyncCommandManager();

        private readonly Dictionary<Type, AbstractAsyncCommand> _activeCommands = new();
        private readonly Dictionary<Type, Queue<AbstractAsyncCommand>> _commandQueues = new();
        private bool _isDisposed = false;

        public async Task<TCommand> ExecuteCommandAsync<TCommand>(
            TCommand command,
            CommandInterruptBehavior interruptBehavior = CommandInterruptBehavior.CancelAndExecuteNew)
            where TCommand : AbstractAsyncCommand
        {
            if (_isDisposed)
                throw new ObjectDisposedException(nameof(AsyncCommandManager));

            if (!command.CanExecute())
                return command;

            var commandType = command.GetType();
            var hasActiveCommand = _activeCommands.ContainsKey(commandType);
            Debug.Log($"Request to execute {commandType.Name} with interrupt behavior {interruptBehavior}, Active Command: {hasActiveCommand}");

            switch (interruptBehavior)
            {
                case CommandInterruptBehavior.IgnoreNew when hasActiveCommand:
                    Debug.Log($"Ignore new {commandType.Name} because one is already running");
                    return command;

                case CommandInterruptBehavior.QueueNew when hasActiveCommand:
                    if (!_commandQueues.ContainsKey(commandType))
                        _commandQueues[commandType] = new Queue<AbstractAsyncCommand>();

                    _commandQueues[commandType].Enqueue(command);
                    Debug.Log($"Queued {commandType.Name}, currently {_commandQueues[commandType].Count} in queue");
                    return command;

                case CommandInterruptBehavior.ExecuteParallel when hasActiveCommand:
                    // 直接执行，不管理之前的命令
                    await ExecuteCommandInternal(command);
                    return command;

                default:
                    // CancelAndExecuteNew 或 CancelCurrentAndExecuteNew
                    if (hasActiveCommand)
                    {
                        Debug.Log($"Canceling current {commandType.Name}");
                        var currentCommand = _activeCommands[commandType];
                        await CancelCommandAsync(currentCommand);
                    }
                    Debug.Log($"Executing new {commandType.Name}");
                    await ExecuteCommandInternal(command);
                    await ProcessNextInQueue(commandType);
                    return command;
            }
        }

        private async Task ExecuteCommandInternal(AbstractAsyncCommand command)
        {
            var commandType = command.GetType();
            Debug.Log($"Starting execution of {commandType.Name}");
            _activeCommands[commandType] = command;

            try
            {
                await command.ExecuteAsync(() => OnCommandCompleted(commandType));
                Debug.Log($"Command {commandType.Name} execution completed");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Command {commandType.Name} execution failed: {ex.Message}");
                _activeCommands.Remove(commandType);
                throw;
            }
        }

        private async Task CancelCommandAsync(AbstractAsyncCommand command)
        {
            if (command.IsRunning)
            {
                // Debug.Log($"Cancelling command {command.GetType().Name}");
                command.Dispose();
                // 给命令一个小的延迟来清理资源
                await Task.Delay(10);
            }
        }

        private void OnCommandCompleted(Type commandType)
        {
            _activeCommands.Remove(commandType);
            _ = ProcessNextInQueue(commandType);
        }

        private async Task ProcessNextInQueue(Type commandType)
        {
            if (_commandQueues.ContainsKey(commandType) && _commandQueues[commandType].Count > 0)
            {
                var nextCommand = _commandQueues[commandType].Dequeue() as AbstractAsyncCommand;
                if (nextCommand != null && nextCommand.CanExecute())
                {
                    Debug.Log($"Executing next queued {commandType.Name}");
                    await ExecuteCommandInternal(nextCommand);
                }
            }
        }

        public bool IsCommandRunning<TCommand>() where TCommand : AbstractAsyncCommand
        {
            return _activeCommands.ContainsKey(typeof(TCommand));
        }

        public async Task CancelAllCommandsAsync()
        {
            var tasks = _activeCommands.Values.Select(cmd => CancelCommandAsync(cmd)).ToArray();
            await Task.WhenAll(tasks);
            _activeCommands.Clear();
            _commandQueues.Clear();
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                _ = CancelAllCommandsAsync();
                _isDisposed = true;
            }
        }
    }
}
