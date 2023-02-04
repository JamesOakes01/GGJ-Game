using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ( Input.GetButton("Horizontal") || Input.GetButton("Vertical") )
        {
            float xAxis = Input.GetAxis("Horizontal");
            float yAxis = Input.GetAxis("Vertical");

            Debug.Log("xAxis: " + xAxis);
            Debug.Log("yAxis: " + yAxis);
        }
    }
}
