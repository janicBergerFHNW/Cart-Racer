using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CartAudio : MonoBehaviour
{
    [Header("Engine")] [SerializeField] private AudioClip enginePurring;

    [SerializeField] private AudioClip engineLow;

    [Header("Tires")] [SerializeField] private AudioClip tiresSquealing;

    private Cart _cart;
    private AudioSource _engineSource;
    private AudioSource _idleSource;
    private AudioSource _tiresSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _engineSource = gameObject.AddComponent<AudioSource>();
        _engineSource.clip = engineLow;
        _engineSource.loop = true;
        _engineSource.volume = 0.5f;
        _engineSource.Play();
        _engineSource.Pause();
        
        _tiresSource = gameObject.AddComponent<AudioSource>();
        _tiresSource.clip = tiresSquealing;
        _tiresSource.loop = true;
        _tiresSource.volume = 0.25f;
        _tiresSource.Play();
        _tiresSource.Pause();
        
        _idleSource = gameObject.AddComponent<AudioSource>();
        _idleSource.clip = enginePurring;
        _idleSource.loop = true;
        _idleSource.volume = 0.4f;
        _idleSource.Play();

        _cart = GetComponentInParent<Cart>();
    }

    // Update is called once per frame
    void Update()
    {
        float slip = _cart.GetBackWheelSlip();
        if (slip > 0)
        {
            _tiresSource.UnPause();
        }
        else
        {
            _tiresSource.Pause();
        }

        _tiresSource.pitch = 1 + (slip * 0.05f);
        
        float speed = _cart.Speed;
        if (speed > 0.5f)
        {
            _engineSource.UnPause();
            _idleSource.Pause();
        }
        else
        {
            _idleSource.UnPause();
            _engineSource.Pause();
        }

        _engineSource.pitch = (1 + speed) * 0.1f;

        if (Time.timeScale == 0)
        {
            _engineSource.Pause();
            _idleSource.Pause();
            _tiresSource.Pause();
        }
    }
}
