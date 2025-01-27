using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraDrag : MonoBehaviour
{
    private Vector3 dragOrigin;
    private Vector3 dragDifference;

    private Camera mainCam;
    private bool isDragging;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void OnDrag(InputAction.CallbackContext context)
    {
        if (context.started) dragOrigin = mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        isDragging = context.started || context.performed;
    }

    void LateUpdate()
    {
        if (!isDragging) return;
        dragDifference = GetMousePosition - transform.position;
        transform.position = dragOrigin - dragDifference;
    }
    private Vector3 GetMousePosition => mainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());
}
