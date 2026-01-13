using System.Collections.Generic;
using Common.Abstract;
using Common.Interface;
using Common.Interface.Base;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.InputSystem;
using static Common.Constants.EventKeys;

public class InputActionEventArgs : IBaseEventArgs
{
    private InputAction.CallbackContext callback;
    public InputAction.CallbackContext Callback => callback;
    public InputActionEventArgs(InputAction.CallbackContext callback)
    {
        this.callback = callback;
    }
}



public partial class InputSystemController : ControllerBehavior
{
    private InputActionMaps inputActionMaps;
    private InputAction RightHold => inputActionMaps.Player.RightHold;
    private InputAction MouseXDrag => inputActionMaps.Player.MouseXDrag;
    private InputAction MouseSelect => inputActionMaps.Player.Select;
    public InputAction Point => inputActionMaps.Player.Point;

    [SerializeField] private bool isRightHold;
    [SerializeField] private float mouseXDelta;
    [SerializeField] private bool isClick;
    public bool IsRightHold => isRightHold;
    public float MouseXDelta => mouseXDelta;
    public bool IsClick => isClick;
    public Vector2 MousePosition => Point.ReadValue<Vector2>();
    private void Awake()
    {
        inputActionMaps = new InputActionMaps();
        inputActionMaps.Player.Enable();
    }

    private void OnEnable()
    {
        RightHold.performed += OnRightHold;
        RightHold.canceled += OnRightHold;
        MouseXDrag.performed += OnMouseXDrag;
        MouseXDrag.canceled += OnMouseXDrag;
        MouseSelect.performed += OnMouseSelect;
        MouseSelect.canceled += OnMouseSelect;
    }
    private void OnDisable()
    {
        RightHold.performed -= OnRightHold;
        RightHold.canceled -= OnRightHold;
        MouseXDrag.performed -= OnMouseXDrag;
        MouseXDrag.canceled -= OnMouseXDrag;
        MouseSelect.performed -= OnMouseSelect;
        MouseSelect.canceled -= OnMouseSelect;
    }

    private void OnMouseSelect(InputAction.CallbackContext context)
    {
        isClick = context.ReadValueAsButton();
        EventPart.Trigger(MouseInput.Select, null, new InputActionEventArgs(context));
    }

    private void OnMouseXDrag(InputAction.CallbackContext context)
    {
        mouseXDelta = context.ReadValue<float>();
        EventPart.Trigger(MouseInput.XDrag, null, new InputActionEventArgs(context));
    }

    private void OnRightHold(InputAction.CallbackContext context)
    {
        isRightHold = context.ReadValueAsButton();
        EventPart.Trigger(MouseInput.RightHold, null, new InputActionEventArgs(context));
    }

}
