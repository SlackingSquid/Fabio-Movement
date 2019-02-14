using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public bool xAxis = false;
    public bool yAxis = false;
    public bool zAxis = false;
    public float speed = 2f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(xAxis)
        {
            transform.Rotate(transform.right, speed * Time.deltaTime,Space.World);
        }
        if (yAxis)
        {
            transform.Rotate(transform.up, speed * Time.deltaTime, Space.World);
        }
        if (zAxis)
        {
            transform.Rotate(transform.forward, speed * Time.deltaTime, Space.World);
        }
    }
}
