using Unity.Properties;
using Common.Abstract;
using ComponentFeature;

namespace UIModel
{
    public class InfraredRangingUIModel : AbstractUIModel
    {
        [CreateProperty]
        public string CurrentState
        {
            get
            {
                return $"当前状态: {(Self as IOTDeviceEntity).CurrentState}";
            }
        }
    }
}

