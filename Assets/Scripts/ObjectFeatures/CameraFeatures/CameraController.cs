using System;
using Core;
using Unity.Cinemachine;
using static Common.Constants.EventKeys;

namespace ComponentFeature
{
    public class CameraController : InputAxisControllerBase<CameraController.Reader>
    {
        private bool isRightHold;

        protected override void OnEnable()
        {
            base.OnEnable();
            EventBus.Instance.Register<InputActionEventArgs>(MouseInput.RightHold, OnRightHold);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            EventBus.Instance.UnRegister<InputActionEventArgs>(MouseInput.RightHold, OnRightHold);
        }

        private void OnRightHold(Entity _, InputActionEventArgs args)
        {
            isRightHold = args.Callback.ReadValueAsButton();
        }

        void Update()
        {
            if (isRightHold)
                UpdateControllers();
        }
        [Serializable]
        public sealed class Reader : IInputAxisReader, IDisposable
        {
            private float mouseXDelta;
            public Reader()
            {
                EventBus.Instance.Register<InputActionEventArgs>(MouseInput.XDrag, OnMouseXDrag);
            }
            public void Dispose()
            {
                EventBus.Instance.UnRegister<InputActionEventArgs>(MouseInput.XDrag, OnMouseXDrag);
            }

            private void OnMouseXDrag(Entity _, InputActionEventArgs args)
            {
                mouseXDelta = args.Callback.ReadValue<float>();
            }

            public float GetValue(UnityEngine.Object context, IInputAxisOwner.AxisDescriptor.Hints hint)
            {
                if (hint == IInputAxisOwner.AxisDescriptor.Hints.X)
                    return mouseXDelta;
                return 0f;
            }

            ~Reader()
            {
                Dispose();
            }
        }
    }
}
