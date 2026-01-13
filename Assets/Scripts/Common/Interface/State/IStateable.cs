using Common.Interface.Base;

namespace Common.Interface
{
    public interface IStateable<T> : IBaseStateable
    {
        T CurState { get; set; }
    }
}
