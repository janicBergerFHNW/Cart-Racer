using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

namespace UI
{

    public class MainMenuUILogic : MonoBehaviour
    {
        private const string LevelSelectorName = "LevelSelector";
        private const string StartButtonName = "StartButton";
        private const string EditCartButtonName = "EditCartButton";
        private const string QuitButtonName = "QuitButton";

        public event EventHandler EditCartButtonPressed;

        protected virtual void OnEditCartButtonPressed()
        {
            EditCartButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        private UIDocument _mainMenuUIDocument;

        private void OnEnable()
        {
            _mainMenuUIDocument = GetComponent<UIDocument>();

            _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
            {
                Debug.Log("Start clicked");
            };
            
            _mainMenuUIDocument.rootVisualElement.Q<Button>(EditCartButtonName).clicked += () =>
            {
                Debug.Log("Edit clicked");
            };
            _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
            {
                Debug.Log("Quit clicked");
            };
        }
    }
}