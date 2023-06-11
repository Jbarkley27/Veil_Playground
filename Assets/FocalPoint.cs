using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocalPoint : MonoBehaviour
{
    // Position of mouse that will change over time
    Vector3 pos = new Vector3(0, 0, 0);

    // Follow Speed
    public float followSpeed = 1f;

    // Distance from player
    public float distance = 100f;

    // Player Object
    public GameObject follower;

    // Input Map
    private InputMap _inputMap;

    // Boundaries
    float boundX = .3f;
    float boundY_Positive = .2f;
    float boundY_Negative = -.6f;
    bool inBoundsX = true;
    bool inBoundsY = true;

    // Mouse Manager Script
    public GameObject mouseManagerObject;
    MouseManager mouseManager;

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
        _inputMap.Gameplay.Aim.performed += OnAim;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        _inputMap.Gameplay.Aim.performed -= OnAim;
    }

    private void Start()
    {
        mouseManager = mouseManagerObject.GetComponent<MouseManager>();
    }


    void Update()
    {
        // Focal Point follow mouse position
        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(pos), Time.deltaTime * followSpeed);
    }

    void checkBounds()
    {
        float mouseX = mouseManager.mousePositionX;
        float mouseY = mouseManager.mousePositionY;

        // Check X Bounds
        if (mouseX >= 0)
        {
            inBoundsX = (mouseX > boundX) ? false : true;
            
        } else
        {
            inBoundsX = (mouseX < -boundX) ? false : true;
        }

        // Check Y Bounds
        if (mouseY >= 0)
        {
            inBoundsY = (mouseY > boundY_Positive) ? false : true;
        }
        else
        {
            inBoundsY = (mouseY < boundY_Negative) ? false : true;
        }
    }

    void OnAim(InputAction.CallbackContext context)
    {
        checkBounds();

        // Update Mouse Position
        Vector2 mouseInput = context.ReadValue<Vector2>();

 
        // Only if Mouse is within bounds change position
        if (inBoundsX)
        {
            pos.x = mouseInput.x;
        } 

        if (inBoundsY)
        {
            pos.y = mouseInput.y;
        }

       
        pos.z = follower.transform.position.z + distance;

    }

}
