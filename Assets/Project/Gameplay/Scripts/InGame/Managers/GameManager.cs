using GameSource.Utils;
using System.Collections;
using UnityEngine;

namespace GameSource.InGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int resumeDuration;
        private int resumeTick;
        private float resumeTimer;

        private void Awake()
        {
            resumeTick = resumeDuration;
            EventManager.Subscribe("OnStartResume", StartResume);
            EventManager.Subscribe("OnGamePaused", PauseGame);
        }

        private void Start()
        {
            EventManager.Trigger("OnLoadGame");
            EventManager.Trigger("OnCurrentScoreChange", 0);
            Time.timeScale = 0;
        }

        private void StartResume(params object[] parameters) { StartCoroutine(ResumeGame()); }
        private void PauseGame(params object[] parameters)
        {
            if ((bool)parameters[0]) Time.timeScale = 0;
            else Time.timeScale = 1;
        }

        private IEnumerator ResumeGame()
        {
            EventManager.Trigger("OnUpdateResume", resumeTick);
            while (resumeTick > 0)
            {
                resumeTimer += Time.unscaledDeltaTime;

                if (resumeTimer >= 1)
                {
                    resumeTick--;
                    resumeTimer = 0;
                    EventManager.Trigger("OnUpdateResume", resumeTick);
                }
                yield return null;
            }

            EventManager.Trigger("OnGamePaused", false);
            resumeTick = resumeDuration;
            resumeTimer = 0;
        }
    }
}

