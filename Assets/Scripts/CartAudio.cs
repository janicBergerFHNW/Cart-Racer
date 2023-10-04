using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CartAudio : MonoBehaviour
{
    [Header("Engine")] [SerializeField] private AudioClip enginePurring;

    [SerializeField] private AudioClip engineLow;

    [Header("Tires")] [SerializeField] private AudioClip tiresSquealing;

    [Header("Boosters")] [SerializeField] private AudioClip initialBoostSfx;
    [SerializeField] private AudioClip continuousBoostSfx;
    [SerializeField] private AudioClip stopBoostSfx;
    [SerializeField] private AudioClip failedBoostSfx;

    [Header("Death")] [SerializeField] private AudioClip deathSfx;
    
    private Cart _cart;
    private AudioSource _engineSource;
    private AudioSource _idleSource;
    private AudioSource _tiresSource;
    private AudioSource _initialBoostSource;
    private AudioSource _continuousBoostSource;
    private AudioSource _stopBoostSource;
    private AudioSource _failedBoostSource;
    private AudioSource _deathSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _engineSource = gameObject.AddComponent<AudioSource>();
        _engineSource.clip = engineLow;
        _engineSource.loop = true;
        _engineSource.volume = 0.1f;
        _engineSource.Play();
        _engineSource.Pause();
        
        _tiresSource = gameObject.AddComponent<AudioSource>();
        _tiresSource.clip = tiresSquealing;
        _tiresSource.loop = true;
        _tiresSource.volume = 0.1f;
        _tiresSource.Play();
        _tiresSource.Pause();
        
        _idleSource = gameObject.AddComponent<AudioSource>();
        _idleSource.clip = enginePurring;
        _idleSource.loop = true;
        _idleSource.volume = 0.1f;
        _idleSource.Play();

        _continuousBoostSource = gameObject.AddComponent<AudioSource>();
        _continuousBoostSource.clip = continuousBoostSfx;
        _continuousBoostSource.loop = true;
        _continuousBoostSource.volume = 1f;
        _continuousBoostSource.Play();

        _stopBoostSource = gameObject.AddComponent<AudioSource>();
        _stopBoostSource.clip = stopBoostSfx;
        _stopBoostSource.loop = false;
        _stopBoostSource.volume = 1f;

        _initialBoostSource = gameObject.AddComponent<AudioSource>();
        _initialBoostSource.clip = initialBoostSfx;
        _initialBoostSource.loop = false;
        _initialBoostSource.volume = 1f;

        _failedBoostSource = gameObject.AddComponent<AudioSource>();
        _failedBoostSource.clip = failedBoostSfx;
        _failedBoostSource.loop = false;
        _failedBoostSource.volume = 0.4f;

        _deathSource = gameObject.AddComponent<AudioSource>();
        _deathSource.clip = deathSfx;
        
        
        _cart = GetComponentInParent<Cart>();
        _cart.InitialBoostEvent += (sender, args) =>
        {
            if (args.Success)
            {
                _initialBoostSource.Play();
            }
            else
            {
                _failedBoostSource.Play();
            }
        };

        _cart.DeathEvent += (_, _) =>
        {
            _deathSource.Play();
        };
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

        _engineSource.pitch = Math.Min(1.5f, (1 + speed) * 0.1f);

        if (_cart.EngineState == EngineState.Boost)
        {
            if (!_continuousBoostSource.isPlaying)
            {
                
                _continuousBoostSource.Play();
            }
        }
        else if (_continuousBoostSource.isPlaying)
        {
            _continuousBoostSource.Stop();
            _stopBoostSource.Play();
        }
        
        if (Time.timeScale == 0)
        {
            _engineSource.Pause();
            _idleSource.Pause();
            _tiresSource.Pause();
            _continuousBoostSource.Pause();
        }
    }
}
