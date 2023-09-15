using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class HomeUIManager : MonoBehaviour
{
    [Header("Main Menu")]
    [SerializeField] private MainMenuUILogic mainMenuPanelPrefab;
    [Space]
    [Header("Cart Editor")]
    [SerializeField] private CartEditorUILogic cartEditorPanelPrefab;

    private MainMenuUILogic _mainMenuPanel;
    private CartEditorUILogic _cartEditorPanel;

    private void Awake()
    {
        _mainMenuPanel = Instantiate(mainMenuPanelPrefab, transform);
        _cartEditorPanel = Instantiate(cartEditorPanelPrefab, transform);
    }

    private void Start()
    {
        _cartEditorPanel.gameObject.SetActive(false);
        _mainMenuPanel.EditCartButtonPressed += OnEditCartButtonPressed;
        _cartEditorPanel.LeaveEditorMenu += OnLeaveEditorMenu;
    }

    private void OnLeaveEditorMenu(object sender, EventArgs e)
    {
        _cartEditorPanel.gameObject.SetActive(false);
        _mainMenuPanel.gameObject.SetActive(true);
    }

    private void OnEditCartButtonPressed(object sender, EventArgs e)
    {
        _mainMenuPanel.gameObject.SetActive(false);
        _cartEditorPanel.gameObject.SetActive(true);
    }
}
