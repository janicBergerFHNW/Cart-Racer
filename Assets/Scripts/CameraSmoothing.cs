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
    private Vector3 _carOffset = new Vector3(0, 2, -2);
    
    void FixedUpdate()
    {
        bool isInWall = false;
        var targetPos = target.position
                        + target.TransformDirection(_offset);
        var camera = GetComponent<Camera>();
        var screenPos = camera.WorldToScreenPoint(target.transform.position);
        var ray = camera.ScreenPointToRay(screenPos);
        var rayOrigin = target.position + target.TransformDirection(_carOffset);
        ray = new Ray(rayOrigin, targetPos - rayOrigin);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Debug.DrawRay (ray.origin, ray.direction *  hit.distance, Color.yellow);
            Debug.Log(hit.rigidbody?.tag);
            if (hit.distance > _offset.magnitude && !hit.rigidbody.CompareTag("Player"))
            {
                targetPos = ray.origin + ray.direction.normalized * (hit.distance * 0.9f);
                isInWall = true;
            }
               // transform.position += transform.TransformDirection(Vector3.forward);
        }
        Debug.DrawLine(transform.position, targetPos, Color.green);
        
        // Define a target position above and behind the target transform
        //Vector3 targetPosition = target.TransformPoint(_offset);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref _velocity, isInWall ? smoothTime : smoothTime);

        // Smoothly move the camera towards that target position
        
        var targetRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
        //transform.LookAt(target);
    }
    
}
