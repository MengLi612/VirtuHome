using Common.Abstract;
using Unity.Properties;

namespace UIModel
{
    public class ScreenUIModel : AbstractUIModel
    {
        [CreateProperty]
        public string ClockFormattedText => GlobalTimer.Instance.GetFormattedTime();
        [CreateProperty]
        public int ClockValue
        {
            get => (int)GlobalTimer.Instance.GlobalTime;
            set => GlobalTimer.Instance.GlobalTime = value;
        }
    }
}
