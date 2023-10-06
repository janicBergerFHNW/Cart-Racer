using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Rondelle : MonoBehaviour
{
    private HashSet<Rigidbody> _rigidbodies = new HashSet<Rigidbody>();
    [FormerlySerializedAs("rotation")] [SerializeField] protected float rps = 0.2f;
    
    private void Start()
    {
    }
    void FixedUpdate()
    {
        transform.eulerAngles += Vector3.up * (rps * Time.deltaTime * 360);
        foreach (var rb in _rigidbodies)
        {
            float radius = (rb.transform.position - transform.position).magnitude;
            float circum = MathF.PI * radius * 2;
            float speed = circum * rps * Time.deltaTime;
            Vector3 vel = speed * Vector3.Cross(transform.position - rb.transform.position, Vector3.up).normalized;

            float force = vel.magnitude / (radius * radius);
            vel += (transform.position - rb.transform.position).normalized * force;
            rb.velocity += vel;
            
            // TODO angular vel
            rb.angularVelocity += Vector3.up * (rps * 2 * MathF.PI * 0.5f * Time.deltaTime);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var rb = other.GetComponent<Rigidbody>();
        _rigidbodies.Add(rb);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        var rb = other.GetComponent<Rigidbody>();
        _rigidbodies.Remove(rb);
    }
}
