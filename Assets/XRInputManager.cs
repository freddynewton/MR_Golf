using UnityEngine;
using UnityEngine.InputSystem;

public class XrInputManager :  Singleton<XrInputManager>
{
    private XRIDefaultInputActions xrInputActions;

    public InputAction GetAction(string actionName)
    {
        return xrInputActions.FindAction(actionName);
    }

    private void OnEnable()
    {
        xrInputActions.Enable();
    }

    private void OnDisable()
    {
        xrInputActions.Disable();
    }

    private void Awake()
    {
        xrInputActions = new XRIDefaultInputActions();
        xrInputActions.Enable();
    }
}
