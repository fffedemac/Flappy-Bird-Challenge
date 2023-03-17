using GameSource.Utils;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSource.InGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private List<GameObject> mainMenuItems = new List<GameObject>();
        [SerializeField] private Text maxScoreMainMenu;

        [SerializeField] private List<GameObject> pauseItems = new List<GameObject>();

        [SerializeField] private List<GameObject> gameOverItems = new List<GameObject>();
        [SerializeField] private Text currentScoreGameOver, maxScoreGameOver;

        [SerializeField] private List<GameObject> gameplayItems = new List<GameObject>();
        [SerializeField] private Text currentScoreGameplay, maxScoreGameplay;

        [SerializeField] private GameObject gameplayResumeItems;
        [SerializeField] private Text gameplayResumeCount;

        private void Awake()
        {
            EventManager.Subscribe("OnCurrentScoreChange", CurrentScoreUpdate);
            EventManager.Subscribe("OnMaxScoreChange", MaxScoreUpdate);
            EventManager.Subscribe("OnUpdateResume", UpdateResumeText);
            EventManager.Subscribe("OnGamePaused", StartGameplay);
            EventManager.Subscribe("OnGameOver", DisplayGameOver);
        }

        private void CurrentScoreUpdate(params object[] parameters)
        {
            currentScoreGameplay.text = ((int)parameters[0]).ToString();
            currentScoreGameOver.text = ((int)parameters[0]).ToString();
        }

        private void MaxScoreUpdate(params object[] parameters)
        {
            maxScoreMainMenu.text = ((int)parameters[0]).ToString();
            maxScoreGameplay.text = ((int)parameters[0]).ToString();
            maxScoreGameOver.text = ((int)parameters[0]).ToString();
        }

        private void UpdateResumeText(params object[] parameters) { gameplayResumeCount.text = ((int)parameters[0]).ToString(); }

        private void StartGameplay(params object[] parameters)
        {
            if ((bool)parameters[0]) return;

            gameplayResumeItems.SetActive(false);
            foreach (GameObject item in gameplayItems) { item.SetActive(true); }
        }

        private void DisplayGameOver(params object[] parameters)
        {
            foreach (GameObject item in gameplayItems) { item.SetActive(false); }
            foreach (GameObject item in gameOverItems) { item.SetActive(true); }
        }

        public void PauseButton()
        {
            EventManager.Trigger("OnGamePaused", true);
            foreach (GameObject item in gameplayItems) { item.SetActive(false); }
            foreach (GameObject item in pauseItems) { item.SetActive(true); }
        }

        public void MainMenuButton()
        {
            foreach (GameObject item in pauseItems) { item.SetActive(false); }
            foreach (GameObject item in gameOverItems) { item.SetActive(false); }
            foreach (GameObject item in mainMenuItems) { item.SetActive(true); }
        }

        public void StartButton()
        {
            foreach (GameObject item in mainMenuItems) { item.SetActive(false); }
            foreach (GameObject item in pauseItems) { item.SetActive(false); }
            foreach (GameObject item in gameOverItems) { item.SetActive(false); }
            foreach (GameObject item in gameplayItems) { item.SetActive(false); }
            gameplayResumeItems.SetActive(true);
            EventManager.Trigger("OnGamePaused", true);
            EventManager.Trigger("OnResetGame");
            EventManager.Trigger("OnStartResume");
        }

        public void ResumeButton()
        {
            foreach (GameObject item in pauseItems) { item.SetActive(false); }
            foreach (GameObject item in gameplayItems) { item.SetActive(false); }
            gameplayResumeItems.SetActive(true);
            EventManager.Trigger("OnStartResume");
        }

        public void ExitButton()
        {
            Application.Quit();
        }
    }
}

