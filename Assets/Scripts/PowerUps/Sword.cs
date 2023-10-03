using System;
using System.Collections;
using UnityEngine;

namespace PowerUps
{
    public class Sword : Summon
    {
        // Start is called before the first frame update
        void Start()
        {
           StartCoroutine(SwordSlash());
        }

        private IEnumerator SwordSlash()
        {
            for (float i = 0; i < 0.2; i += Time.fixedDeltaTime)
            {
                transform.eulerAngles += Vector3.up * (Time.fixedDeltaTime * 280 / 0.2f);
                yield return null;
            }
            Destroy(gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            var otherCart = other.GetComponent<Cart>();
            if (otherCart == User) return;
            if (otherCart.isDead) return;
            
            other.attachedRigidbody.AddForce(transform.right * 500000);
            otherCart.Die();
            User.KillCount += 1;
        }
    }
}
