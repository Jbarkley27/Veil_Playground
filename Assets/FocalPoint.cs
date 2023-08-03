using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class FocalPoint : MonoBehaviour
{
    // Position of mouse that will change over time
    Vector3 pos = new Vector3((Screen.width /2), (Screen.height / 2), 0);

    // Current Joystick direction
    Vector3 currentJoystickPos = new Vector3(0, 0, 0);

    // Joystick value multiplier for focal point follow speed;
    public float joystickMultiplier = 2f;

    public GameObject followMeBro;

    // Follow Speed
    public float followSpeed = 1f;

    // Cam Follow Speed
    public float camSpeed = 2f;

    public GameObject focal;

    public bool lookAtPlayerFocalPoint = true;

    // Distance from player
    public float distance = 1000000f;

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

    public GameObject player;

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
        _inputMap.Gameplay.AimMouse.performed += AimWithMouse;
        _inputMap.Gameplay.AimGamepad.performed += AimWithGamepad;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        _inputMap.Gameplay.AimGamepad.performed -= AimWithGamepad;
        _inputMap.Gameplay.AimMouse.performed -= AimWithMouse;
    }

    private void Start()
    {
        mouseManager = mouseManagerObject.GetComponent<MouseManager>();
        pos.z = follower.transform.position.z + distance;
    }


    void Update()
    {

        // When using gamepad we need to constantly be adding the vector since
        // unlike mouse position that overrides the pos vector, the gamepad values
        // are normalized so we need to constantly add that normalized vector to move
        // the focal point to its desired position
        pos.x += currentJoystickPos.x * 100f;
        pos.y += currentJoystickPos.y * 100f;
        pos.z = follower.transform.position.z + distance;

        transform.position = Vector3.Slerp(transform.position, Camera.main.ScreenToWorldPoint(pos), Time.deltaTime * followSpeed);
    }

    //private void LateUpdate()
    //{
    //    Vector3 moveCamTo = player.transform.position - player.transform.forward * 45f + Vector3.up * 8.0f;

    //    float bias = 1f;

    //    Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position * bias +
    //                                                        moveCamTo * (1.0f - bias), Time.deltaTime * camSpeed);

    //    Camera.main.transform.LookAt(player.transform.position + player.transform.forward * 10f);

    //}

    void checkBounds()
    {
        float mouseX = mouseManager.movementInputX;
        float mouseY = mouseManager.movementInputY;

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


    void AimWithMouse(InputAction.CallbackContext context)
    {
        checkBounds();
        Debug.Log("------" + "Using Mouse");

        // Reset joystick position
        currentJoystickPos.x = 0;
        currentJoystickPos.y = 0;

        // Update Mouse Position
        Vector2 mouseInput = context.ReadValue<Vector2>();
        

        pos.x = mouseInput.x;
        pos.y = mouseInput.y;
    }

    void AimWithGamepad(InputAction.CallbackContext context)
    {

        checkBounds();
        Debug.Log("------" + "Using Gamepad");


        // Update Mouse Position
        Vector2 mouseInput = context.ReadValue<Vector2>();

        currentJoystickPos.x = mouseInput.x * joystickMultiplier;
        currentJoystickPos.y = mouseInput.y * joystickMultiplier;
    }

}
