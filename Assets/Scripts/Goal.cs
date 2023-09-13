using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material highlightMaterial;
    private MeshRenderer[] _renderers;
    public Vector3 resetOffset;
    public Vector3 ResetPosition => transform.position + transform.TransformDirection(resetOffset);

    private void Awake()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
        Unmark();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }

        var cart = other.GetComponentInParent<Cart>();
        OnGoalPassed(cart);
    }

    public event EventHandler<GoalEA> GoalPassed;

    protected virtual void OnGoalPassed(Cart cart)
    {
        var ea = new GoalEA();
        ea.Goal = this;
        ea.Cart = cart;
        GoalPassed?.Invoke(this, ea);
    }

    public class GoalEA : EventArgs
    {
        public Goal Goal { get; set; }
        public Cart Cart { get; set; }
    }

    public void MarkAsNext()
    {
        foreach (var renderer in _renderers)
        {
            Debug.Log(renderer);
            renderer.material = highlightMaterial;
            Debug.Log(renderer.material);
        }
    }

    public void Unmark()
    {
        foreach (var renderer in _renderers)
        {
            renderer.material = defaultMaterial;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(ResetPosition, 0.5f); 
    }
}