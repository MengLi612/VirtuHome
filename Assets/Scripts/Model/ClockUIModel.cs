using Common.Abstract;
using Common.Enum;
using ComponentFeature;
using Sirenix.OdinInspector;
using Unity.Properties;

namespace UIModel
{
    public sealed class ClockUIModel : AbstractUIModel
    {
        [ShowInInspector]
        private Timer Timer => Self.GetPart<Timer>();

        [CreateProperty]
        [ShowInInspector]
        public string TimeString => Timer.GetFormattedCurTime(TimeFormat.HHmmss);
    }
}
