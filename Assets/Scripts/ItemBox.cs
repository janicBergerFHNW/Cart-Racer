using System;
using System.Collections;
using System.Collections.Generic;
using PowerUps;
using UnityEngine;
using Random = System.Random;

public class ItemBox : MonoBehaviour
{
    public class ItemBoxCollectedEventArgs
    {
        public Cart Cart;
        public PowerUp PowerUp;
    }

    public event EventHandler<ItemBoxCollectedEventArgs> ItemBoxCollected;

    private ParticleSystem _ps;
    private AudioSource _audioSource;

    [SerializeField] private List<PowerUp> powerUpPrefabs;
    
    // Start is called before the first frame update
    void Start()
    {
        _ps = GetComponentInChildren<ParticleSystem>();
        _audioSource = _ps.GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        var rand = new Random();
        int i = rand.Next(powerUpPrefabs.Count);
        var powerUp = powerUpPrefabs[i];
        
        var ea = new ItemBoxCollectedEventArgs();
        ea.Cart = other.GetComponent<Cart>();
        ea.Cart.PickUpPowerUp(powerUp);
        ea.PowerUp = powerUp;
        powerUp.User = ea.Cart;
        
        ItemBoxCollected?.Invoke(this, ea);
        
        _ps.transform.parent = null;
        
        _ps.Play();
        _audioSource.Play();
        Destroy(_ps, 1);
        Destroy(_audioSource, 1);
        
        Destroy(gameObject);
    }
}
