using Core;
using static Common.Constants.EventKeys;

public class BaseLifecycleUpdate : ControllerBehavior
{
    private void OnEnable()
    {
        EventPart.Trigger(Lifecycle.OnEnable, null, null);
    }
    private void OnDisable()
    {
        EventPart.Trigger(Lifecycle.OnDisable, null, null);
    }
    private void Update()
    {
        EventPart.Trigger(Lifecycle.Update, null, null);
    }
    private void FixedUpdate()
    {
        EventPart.Trigger(Lifecycle.FixedUpdate, null, null);
    }
}

