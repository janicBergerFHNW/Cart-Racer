using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace UI
{
    public class OverlayUILogic : MonoBehaviour
    {
        private const string MainMenuButtonName = "MainMenu";
        private const string EditCartButtonName = "EditCart";

        public event EventHandler EditCartButtonPressed;

        protected virtual void OnEditCartButtonPressed()
        {
            EditCartButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        private UIDocument _overlayDocument;

        private void OnEnable()
        {
            _overlayDocument = GetComponent<UIDocument>();
            
            _overlayDocument.rootVisualElement.Q<Button>(MainMenuButtonName).clicked += () =>
            {
                Debug.Log("Main clicked");
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            };
            
            _overlayDocument.rootVisualElement.Q<Button>(EditCartButtonName).clicked += () =>
            {
                Debug.Log("Edit clicked");
                OnEditCartButtonPressed();
            };
        }
    }
}