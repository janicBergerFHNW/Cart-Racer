using System;
using UnityEngine;

namespace UI
{
    public class CameraLookAtSmoothing : MonoBehaviour
    {
        
        [SerializeField] public Transform target;
        [SerializeField] private float rotationSpeed = 10;

        private void FixedUpdate()
        {
            var targetRot = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.fixedDeltaTime * rotationSpeed);
        
        }
    }
}