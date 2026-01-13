using Common.Enum;
using ComponentFeature;

namespace Common.Interface
{
    public interface IManageOtherStateable
    {
        public void SetIOTDeviceState(IOTDeviceEntity iotDevice, SensorState state);
    }
}
