using Common.Enum;
using Sirenix.OdinInspector;

namespace Common.Concrete.Unit
{

    public record IOTDeviceState
    {
        [EnumToggleButtons]
        public LinkStateType LinkState;
        [EnumToggleButtons]
        public OperatingStateType State;
        public IOTDeviceState(OperatingStateType state = OperatingStateType.Disable, LinkStateType linkState = LinkStateType.Disconnected)
        {
            State = state;
            LinkState = linkState;
        }
    }
}