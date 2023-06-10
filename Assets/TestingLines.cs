using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingLines : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        DrawLines("z", 5, Color.magenta);
        DrawLines("y", 5, Color.cyan);
        DrawLines("x", 5, Color.black);
    }

    void DrawLines(string axis, float length, Color color)
    {
        Vector3 startForward = transform.position;
        Vector3 endForwad = transform.position;
        switch (axis)
        {
            case "z":
                startForward.z -= length;
                endForwad.z += length;
                break;
            case "x":
                startForward.x -= length;
                endForwad.x += length;
                break;
            case "y":
                startForward.y -= length;
                endForwad.y += length;
                break;
            default:
                break;
        }
        Debug.DrawLine(startForward, endForwad, color);
    }
}
