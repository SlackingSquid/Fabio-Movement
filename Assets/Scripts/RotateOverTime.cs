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
            transform.RotateAroundLocal(transform.right, speed * Time.deltaTime);
        }
        if (yAxis)
        {
            transform.RotateAroundLocal(transform.up, speed * Time.deltaTime);
        }
        if (zAxis)
        {
            transform.RotateAroundLocal(transform.forward, speed * Time.deltaTime);
        }
    }
}
