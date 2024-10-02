using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTrap : MonoBehaviour
{

    private int rotateSpeed = 5;
    void Update()
    {
        transform.Rotate(rotateSpeed, 0f, 0f, Space.World);
        
    }
}
