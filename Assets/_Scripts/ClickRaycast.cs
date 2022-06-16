using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 
/// </summary>
public class ClickRaycast : MonoBehaviour
{
    [SerializeField, Tooltip("Input Action that will trigger the drag.")]
    private InputAction mouseClick;
    [SerializeField]
    private Camera mainCamera;

    private void OnEnable() {
        mouseClick.Enable();
        mouseClick.performed += MousePressed;
    }   

    private void OnDisable() {
        mouseClick.performed -= MousePressed;
        mouseClick.Disable();
    }

    /// <summary>
    /// Called when the mouseClick action is performed.
    /// Performs a raycast (shoots a ray from the camera) from the mouse screen coordinates and calls IClick interface if valid.
    /// <param name="context">Information regarding the action.</param>
    private void MousePressed(InputAction.CallbackContext context) {
        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        // 3D Collisions.
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            if (hit.collider != null && hit.collider.gameObject.GetComponent<IClick>() != null) {
                 hit.collider.gameObject.GetComponent<IClick>().OnClick();
            }
        }
    }
}
