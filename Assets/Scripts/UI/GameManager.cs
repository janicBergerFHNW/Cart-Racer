using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PauseMenuUILogic pauseMenuPrefab;

    private PauseMenuUILogic _pauseMenuUIPanel;

    private void Awake()
    {
        _pauseMenuUIPanel = Instantiate(pauseMenuPrefab, transform);
    }

    // Start is called before the first frame update
    void Start()
    {
        _pauseMenuUIPanel.gameObject.SetActive(false);
        _pauseMenuUIPanel.ResumeEvent += (_, _) => OnResume();
    }

    private bool _isPaused;
    private float _prePauseTimeScale = 1;

    public void OnPause(InputAction.CallbackContext ctx)
    { 
        Debug.Log(_isPaused);
        if (_isPaused)
        {
            OnResume();
        }
        else
        {
            _prePauseTimeScale = Time.timeScale;
            Time.timeScale = 0;
            _pauseMenuUIPanel.gameObject.SetActive(true);
            _isPaused = true;
        }
    }

    private void OnResume()
    {
        _pauseMenuUIPanel.gameObject.SetActive(false);
        Time.timeScale = _prePauseTimeScale;
        _isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
