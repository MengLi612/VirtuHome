namespace Common.Interface
{
    public interface ISingletonable<T>
    {
        static T Instance { get; }
    }
}
