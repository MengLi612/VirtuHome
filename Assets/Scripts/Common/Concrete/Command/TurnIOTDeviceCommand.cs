using Common.Abstract;
using Common.Concrete.Unit;
using ComponentFeature;

namespace Common.Concrete.Command
{
    public class TurnIOTDeviceCommand : AbstractCommand
    {
        private readonly IOTDeviceEntity _iotDevice;
        private readonly IOTDeviceState _state;
        public TurnIOTDeviceCommand(IOTDeviceEntity iotDevice, IOTDeviceState state)
        {
            _iotDevice = iotDevice;
            _state = state;
        }

        public override bool CanExecute()
        {
            if (_iotDevice == null)
            {
                return false;
            }
            return true;
        }

        public override void Execute()
        {
            _iotDevice.CurrentState = _state.State;
        }
    }
}
