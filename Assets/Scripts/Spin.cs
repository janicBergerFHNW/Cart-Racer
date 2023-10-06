using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Spin : MonoBehaviour
{
    [SerializeField] protected Vector3 rotation = new Vector3(0, 1, 0);
    void FixedUpdate()
    {
        transform.eulerAngles += rotation;

    }
}
