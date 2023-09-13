using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UIElements.Button;

namespace UI
{

    public class MainMenuUILogic : MonoBehaviour
    {
        private const string LevelSelectorName = "LevelSelector";
        private const string StartButtonName = "StartButton";
        private const string EditCartButtonName = "EditCartButton";
        private const string QuitButtonName = "QuitButton";

        [SerializeField] private GameObject[] _previews;
        
        public event EventHandler EditCartButtonPressed;

        protected virtual void OnEditCartButtonPressed()
        {
            EditCartButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        private UIDocument _mainMenuUIDocument;

        private void Awake()
        {
            disablePreviews();
        }

        private void OnEnable()
        {
            _mainMenuUIDocument = GetComponent<UIDocument>();

            DropdownField dropdown = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>();
            
            _previews[dropdown.index].SetActive(true);
            dropdown.RegisterValueChangedCallback(v =>
            {
                disablePreviews();
                _previews[dropdown.index].SetActive(true);
            });
            

            _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
            {
                Debug.Log("Start clicked");
                int sceneIndex = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>().index + 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
            };
            
            _mainMenuUIDocument.rootVisualElement.Q<Button>(EditCartButtonName).clicked += () =>
            {
                Debug.Log("Edit clicked");
                OnEditCartButtonPressed();
            };
            
            _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
            {
                Debug.Log("Quit clicked");
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            };
        }

        private void disablePreviews()
        {
            foreach (var preview in _previews)
            {
                preview.SetActive(false);
            }
        }
    }
}