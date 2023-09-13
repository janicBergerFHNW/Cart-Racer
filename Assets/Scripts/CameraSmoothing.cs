using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothing : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float smoothTime = 0.3F;
    [SerializeField] private float rotationSpeed = 10F;
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private Vector3 _offset = new Vector3(0, 5, -10);
    
    void FixedUpdate()
    {
        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(_offset);
        var targetPos = target.position
                        + target.TransformDirection(_offset);

        // Smoothly move the camera towards that target position
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, smoothTime);
        
        var targetRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
        //transform.LookAt(target);
    }
    
}
