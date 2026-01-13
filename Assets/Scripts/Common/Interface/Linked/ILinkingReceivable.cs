using Common.Abstract;
using Common.Concrete.Unit;

namespace Common.Interface
{
    public interface ILinkingReceivable
    {
        void MsgReceive(ILinkingSendable sendable, IOTMessage msg);
    }
}
