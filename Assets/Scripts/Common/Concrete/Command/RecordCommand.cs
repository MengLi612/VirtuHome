using System;
using Common.Abstract;
using Common.Interface;

namespace Common.Concrete.Command
{
    public class RecordCommand<T> : AbstractCommand
    {
        private readonly IRecordable<T> _recordable;
        private readonly T _data;

        public RecordCommand(IRecordable<T> recordable, T data)
        {
            _recordable = recordable;
            _data = data;
        }
        public override bool CanExecute()
        {
            if (_recordable == null) return false;

            return true;
        }

        public override void Execute()
        {
            _recordable.Records.Add(_data);
        }
    }
}
