using System;
using UnityEngine;

namespace PowerUps
{
    public class Fireball : Summon
    {
        [SerializeField] private float speed = 100;
        [SerializeField] private AudioClip explosionSfx;

        private Rigidbody _rigidbody;
        // Start is called before the first frame update
        void Start()
        {
            transform.parent = null;
            _rigidbody = GetComponent<Rigidbody>();
            var cartForwardVelocity = Vector3.Dot(User.transform.forward, User.GetComponent<Rigidbody>().velocity) * User.transform.forward;
            _rigidbody.velocity = cartForwardVelocity + User.transform.forward * speed;
        }

        private Collider _lastReflector = null;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name);
            if (other.CompareTag("IgnoreCollision")) return;
            if (other.CompareTag("Reflector") && other != _lastReflector)
            {
                Debug.Log("DEFLECTED");
                _lastReflector = other;
                User = other.GetComponent<Summon>().User;
                _rigidbody.velocity *= -1;
                return;
            }
            var explosionParticles = GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in explosionParticles)
            {
                ps.transform.parent = null;
                ps.Play();
                Destroy(ps.gameObject, 2);
            }

            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = explosionSfx;
            audioSource.Play();
            audioSource.transform.parent = null;
            Destroy(audioSource.gameObject, 2);
            
            if (other.CompareTag("Player"))
            {
                var cart = other.GetComponent<Cart>();
                cart.Die();
                if (cart != User && !cart.isDead)
                {
                    User.KillCount++;
                }
            }
            
            Destroy(gameObject);
        }
    }
}
