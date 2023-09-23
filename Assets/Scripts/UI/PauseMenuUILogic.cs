using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace UI
{
    public class PauseMenuUILogic : MonoBehaviour
    {
        private const string ResumeButtonName = "ResumeButton";
        private const string MainMenuButtonName = "MainMenuButton";

        public EventHandler ResumeEvent;

        private UIDocument _pauseMenuUIDocument;

        private void OnEnable()
        {
            _pauseMenuUIDocument = GetComponent<UIDocument>();

            _pauseMenuUIDocument.rootVisualElement.Q<Button>(ResumeButtonName).clicked += () =>
            {
                ResumeEvent?.Invoke(this, EventArgs.Empty);
            };

            _pauseMenuUIDocument.rootVisualElement.Q<Button>(MainMenuButtonName).clicked += () =>
            {
                Time.timeScale = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            };
        }
    }
}