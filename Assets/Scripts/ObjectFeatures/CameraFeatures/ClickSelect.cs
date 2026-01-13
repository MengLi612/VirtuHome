using Core;
using UnityEngine;
using UnityEngine.InputSystem;
using static Common.Constants.EventKeys;

namespace ComponentFeature
{
    public class ClickSelect : ControllerBehavior
    {
        public Camera MainCamera => Camera.main;
        private InputAction Point => GetController<InputSystemController>().Point;

        private void OnEnable()
        {
            EventPart.Register<InputActionEventArgs>(MouseInput.Select, OnMouseSelect);
        }

        private void OnDisable()
        {
            EventPart.UnRegister<InputActionEventArgs>(MouseInput.Select, OnMouseSelect);
        }

        private void OnMouseSelect(Entity entity, InputActionEventArgs args)
        {
            if (args.Callback.performed)
            {
                Ray ray = MainCamera.ScreenPointToRay(Point.ReadValue<Vector2>());
                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    var hitObject = hitInfo.collider.gameObject;
                    Debug.Log($"Clicked on: {hitObject.name}");
                    EventPart.Trigger(MouseInput.SelectClick + hitObject.GetInstanceID(), hitObject.GetComponentInParent<Entity>(), hitInfo);
                }
            }
        }
    }
}
