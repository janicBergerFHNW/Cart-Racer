using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpin : MonoBehaviour
{
    private Vector3 _rotation = new Vector3(0, 1, 0);
    void FixedUpdate()
    {
        transform.eulerAngles += _rotation;

    }
}
