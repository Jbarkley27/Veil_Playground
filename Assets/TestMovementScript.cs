using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestMovementScript : MonoBehaviour
{
    // Input Map
    private InputMap _inputMap;

    public float precision = 100f;
    public float mousePositionX = 0f;
    public float mousePositionY = 0f;

    private void Awake()
    {
        // Initialize Input Map
        if (_inputMap == null) _inputMap = new InputMap();
    }

    private void OnEnable()
    {
        // Enable Input Map
        _inputMap.Enable();

        // Subscribe to Events
        _inputMap.Gameplay.AimMouse.performed += MousePosition;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        _inputMap.Gameplay.AimMouse.performed -= MousePosition;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 30f;

        transform.Rotate(-mousePositionY, mousePositionX, 0);
    }

    void MousePosition(InputAction.CallbackContext context)
    {
        // Get the mouse position in screen coordinates
        Vector3 mouseScreenPosition = context.ReadValue<Vector2>();

        // Convert mouse position to viewport coordinates
        Vector3 mouseViewportPosition = Camera.main.ScreenToViewportPoint(mouseScreenPosition);

        // Clamp the mouse position to the game viewport
        float clampedX = Mathf.Clamp(mouseViewportPosition.x, 0f, 1f);
        float clampedY = Mathf.Clamp(mouseViewportPosition.y, 0f, 1f);

        // Calculate the relative position based on the clamped viewport coordinates with center as (0, 0)
        float mouseX = (clampedX - 0.5f) * 2f;
        float mouseY = (clampedY - 0.5f) * 2f;

        // Round the relative coordinates to the nearest tenth place
        //mousePositionX = Mathf.Round(mouseX * precision) / precision;
        //mousePositionY = Mathf.Round(mouseY * precision) / precision;

        mousePositionX = mouseScreenPosition.x;
        mousePositionY = mouseScreenPosition.y;

        // Print the rounded mouse position relative to the game viewport with center as (0, 0)
        //Debug.Log("Mouse Position (Rounded): " + mousePositionX + ", " + mousePositionY);
    }
}
