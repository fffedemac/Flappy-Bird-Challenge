using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameSource.Utils;
using GameSource.Serialization;

namespace GameSource.InGame
{
    public class ScoreManager : MonoBehaviour
    {
        private int currentScore, highScore;

        private void Awake()
        {
            EventManager.Subscribe("OnPipePass", ScoreUpdate);
            EventManager.Subscribe("OnStartResume", UpdateScoreText);
            EventManager.Subscribe("OnResetGame", ResetScore);
            EventManager.Subscribe("OnGameOver", SaveScore);
            EventManager.Subscribe("OnLoadGame", LoadScore);
        }

        private void ScoreUpdate(params object[] parameters)
        {
            currentScore += 1;
            EventManager.Trigger("OnCurrentScoreChange", currentScore);

            if (currentScore > highScore)
            { 
                highScore = currentScore;
                EventManager.Trigger("OnMaxScoreChange", highScore);
            }
        }

        private void ResetScore(params object[] parameters){ currentScore = 0; }
        private void UpdateScoreText(params object[] parameters) { EventManager.Trigger("OnCurrentScoreChange", currentScore); }

        public void SaveScore(params object[] parameters)
        {
            HighScoreData data = new HighScoreData();
            data.highScore = highScore;
            BinarySerializer.SaveBinary(data, $"{Application.dataPath}\\highScore.dat");
        }

        public void LoadScore(params object[] parameters)
        {
            HighScoreData data = BinarySerializer.LoadBinary<HighScoreData>($"{Application.dataPath}\\highScore.dat");

            if (data != null) highScore = data.highScore;
            else highScore = 0;

            EventManager.Trigger("OnMaxScoreChange", highScore);
        }
    }
}

