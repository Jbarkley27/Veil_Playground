using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody rb;
    public float speed = 10f;

    // Cam Follow Speed
    public float camSpeed = 5f;

    // Neutral Bounds
    public float neutral_bounds = 0.15f;

    // Medium Bounds
    public float medium_bounds = 0.7f;

    public enum Bounds { NEUTRAL, INNER, OUTER };

    public Bounds current_bounds_x = Bounds.NEUTRAL;
    public Bounds current_bounds_y = Bounds.NEUTRAL;

    // Mouse Manager Script
    public GameObject mouseManagerObject;
    MouseManager mouseManager;

    // Accelerate
    public bool should_accelerate = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        mouseManager = mouseManagerObject.GetComponent<MouseManager>();
    }

    // Input Map
    private InputMap _inputMap;

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
        _inputMap.Gameplay.Accelerate.performed += Accelerate;
        _inputMap.Gameplay.Accelerate.canceled += Accelerate;
    }

    private void OnDisable()
    {
        // Unsubscribe from events
        _inputMap.Gameplay.Accelerate.performed -= Accelerate;
        _inputMap.Gameplay.Accelerate.canceled -= Accelerate;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        
        

        float x = mouseManager.movementInputX;
        float y = mouseManager.movementInputY;

        //rb.constraints = RigidbodyConstraints.FreezePositionZ;

        //target.transform.Rotate(new Vector3(vertical, -horizontal, 0) * Time.deltaTime);
        

        transform.Rotate(current_bounds_y != Bounds.NEUTRAL ? -y : 0f, current_bounds_x != Bounds.NEUTRAL ? x : 0f, 0f);
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);

        Vector3 moveCamTo = transform.position - transform.forward * 30f + Vector3.up * 10.0f;

        float bias = .98f;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, Camera.main.transform.position * bias +
                                                            moveCamTo * (1.0f - bias), Time.deltaTime * camSpeed);


        Camera.main.transform.LookAt(transform.position + transform.forward * 20f);
        readBounds();


        //if (should_accelerate)
        //{
        //    //rb.AddForce(expectedVelocity - rb.velocity, ForceMode.VelocityChange);
        //    rb.AddForce(transform.forward * 10f);
        //}


    }


    void Accelerate(InputAction.CallbackContext context)
    {

        should_accelerate = context.ReadValue<float>() == 1;

        //rb.AddForce(expectedVelocity, ForceMode.VelocityChange);
        Debug.Log("Accerating" + context.ReadValue<float>()); ;
    }





    void readBounds()
    {

        float x = Mathf.Abs(mouseManager.movementInputX);
        float y = Mathf.Abs(mouseManager.movementInputY);

        if (x < neutral_bounds)
        {
            current_bounds_x = Bounds.NEUTRAL;

        } else if (x >= neutral_bounds && x <= medium_bounds)
        {
            current_bounds_x = Bounds.INNER;
        } else
        {
            current_bounds_x = Bounds.OUTER;
        }


        if (y < neutral_bounds)
        {
            current_bounds_y = Bounds.NEUTRAL;

        }
        else if (y >= neutral_bounds && y <= medium_bounds)
        {
            current_bounds_y = Bounds.INNER;
        }
        else
        {
            current_bounds_y = Bounds.OUTER;
        }

        Debug.Log("X Bounds ---- " + current_bounds_x);
        Debug.Log("Y Bounds ---- " + current_bounds_y);
    }
}
