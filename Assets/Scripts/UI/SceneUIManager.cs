using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

public class SceneUIManager : MonoBehaviour
{
    [Header("Overlay")] 
    [SerializeField] private OverlayUILogic overlayPanelPrefab;

    [Space]
    [Header("Cart Editor")] 
    [SerializeField] private CartEditorUILogic cartEditorPanelPrefab;

    private OverlayUILogic _overlayPanel;
    private CartEditorUILogic _cartEditorPanel;

    private void Awake()
    {
        _overlayPanel = Instantiate(overlayPanelPrefab, transform);
        _cartEditorPanel = Instantiate(cartEditorPanelPrefab, transform);
    }

    private void Start()
    {
        _cartEditorPanel.gameObject.SetActive(false);
        _overlayPanel.EditCartButtonPressed += OnEditCartButtonPressed;
        _cartEditorPanel.LeaveEditorMenu += OnLeaveEditorMenu;
    }

    private float _tempTimeScale;
    
    private void OnLeaveEditorMenu(object sender, EventArgs e)
    {
        Time.timeScale = _tempTimeScale;
        _cartEditorPanel.gameObject.SetActive(false);
        _overlayPanel.gameObject.SetActive(true);
    }

    private void OnEditCartButtonPressed(object sender, EventArgs e)
    {
        _tempTimeScale = Time.timeScale;
        Time.timeScale = 0;
        _overlayPanel.gameObject.SetActive(false);
        _cartEditorPanel.gameObject.SetActive(true);
    }

}