using Common.Concrete.Unit;

namespace Common.Interface
{
    public interface ILinkingSendable
    {
        void MsgSend(IOTMessage msg);
    }
}

