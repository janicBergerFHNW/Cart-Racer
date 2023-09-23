using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        [SerializeField] private Track[] trackPrefabs;

        private Track[] _tracks;
        
        public event EventHandler EditCartButtonPressed;

        protected virtual void OnEditCartButtonPressed()
        {
            EditCartButtonPressed?.Invoke(this, EventArgs.Empty);
        }

        private UIDocument _mainMenuUIDocument;

        private void Awake()
        {
            _tracks = trackPrefabs;
            for (int i = 1; i < trackPrefabs.Length; i++)
            {
                _tracks[i].gameObject.SetActive(false);
            }
        }

        private void OnEnable()
        {
            _mainMenuUIDocument = GetComponent<UIDocument>();

            DropdownField dropdown = _mainMenuUIDocument.rootVisualElement.Q<DropdownField>(LevelSelectorName);
            dropdown.choices = _tracks.Select(t => t.trackName).ToList();
            dropdown.index = 0;
            _tracks[0].gameObject.SetActive(true);
            dropdown.RegisterValueChangedCallback(v =>
            {
                Debug.Log($"from {v.previousValue} to {v.newValue}");
                _tracks.Where(t => t.trackName == v.previousValue).First().gameObject.SetActive(false);
                _tracks.Where(t => t.trackName == v.newValue).First().gameObject.SetActive(true);
            });
            

            _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName).clicked += () =>
            {
                Debug.Log("Start clicked");
                int sceneIndex = dropdown.index + 1;
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
    }
}