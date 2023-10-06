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
        private const string PreviousStageButtonName = "PreviousStageButton";
        private const string NextStageButtonName = "NextStageButton";
        private const string StartButtonName = "StartButton";
        private const string QuitButtonName = "QuitButton";

        [SerializeField] private Track[] tracks;
        [SerializeField] private CameraLookAtSmoothing camera;

        private int _i = 0;
        private Button _startButton;

        private UIDocument _mainMenuUIDocument;

        private void OnEnable()
        {
            _mainMenuUIDocument = GetComponent<UIDocument>();

            camera.target = tracks[_i].transform;

            _startButton = _mainMenuUIDocument.rootVisualElement.Q<Button>(StartButtonName);
            _startButton.clicked += () =>
            {
                int sceneIndex = _i + 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneIndex);
            };
            
            _startButton.text = tracks[_i].trackName.ToUpper();
            _mainMenuUIDocument.rootVisualElement.Q<Button>(PreviousStageButtonName).clicked += () =>
            {
                Debug.Log("prev");
                _i = (_i - 1 + tracks.Length) % tracks.Length;
                camera.target = tracks[_i].transform;
                _startButton.text = tracks[_i].trackName.ToUpper();
            };
            _mainMenuUIDocument.rootVisualElement.Q<Button>(NextStageButtonName).clicked += () =>
            {
                Debug.Log("next");
                _i = (_i + 1) % tracks.Length;
                camera.target = tracks[_i].transform;
                _startButton.text = tracks[_i].trackName.ToUpper();
            };
            
            _mainMenuUIDocument.rootVisualElement.Q<Button>(QuitButtonName).clicked += () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            };
        }
    }
}