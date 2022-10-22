using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using IACFPSController.Managers;


namespace IACFPSController.Managers
{

    [AddComponentMenu("IAC Axe Game/Managers/End Game Manager")]
    public class EndGameManager : MonoBehaviour
    {
        public enum WinConditions { ScoreBased, ReachDestination };
        public WinConditions winCondition;

        public KeyCode resetTopScoresKey = KeyCode.F9;

        public EndGameEvents endEvents;
        private TimeManager timeManager;
        private ScoreManager scoreManager;


        [Serializable]
        public class EndGameEvents
        {
            public UnityEvent scoreWinEvent;
            public UnityEvent DestinationReachedEvent;
            public UnityEvent TimeOverEvent;
        }

        private void Awake()
        {
            timeManager = FindObjectOfType<TimeManager>();
            scoreManager = FindObjectOfType<ScoreManager>();
          
        }

        private void Start()
        {
            switch (winCondition)
            {
                case WinConditions.ScoreBased:
                    scoreManager.checkForWinScore = true;
                    break;

                case WinConditions.ReachDestination:
                    scoreManager.checkForWinScore = false;
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            if (Input.GetKeyDown(resetTopScoresKey))
            {
                PlayerPrefs.SetInt("TopScore", 0);
                PlayerPrefs.SetFloat("TopTime", 0);
            }
        }


        public void ScoreReached()
        {
            if (timeManager)
            {
                UpdateEndGameTime();
            }

            if(scoreManager)
            {
                UpdateEndGameScore();
            }

            endEvents.scoreWinEvent.Invoke();
            ToggleMouseCursor(true);
        }

        public void DestinationReached()
        {
            if(scoreManager)
            {
                UpdateEndGameScore();
            }
            if(timeManager)
            {
                UpdateEndGameTime();
            }

            endEvents.DestinationReachedEvent.Invoke();
            ToggleMouseCursor(true);
        }


        public void TimeEnded()
        {
            if(scoreManager)
            {
                UpdateEndGameScore();
            }

            if(timeManager)
            {
                UpdateEndGameTime();
            }

            endEvents.TimeOverEvent.Invoke();
            ToggleMouseCursor(true);
        }



        public void UpdateEndGameScore()
        {
            int topScore = PlayerPrefs.GetInt("TopScore");
            int currentScore = scoreManager.GetCurrentScore();
            PlayerPrefs.SetInt("EndGameScore", currentScore);

            if (currentScore >= topScore)
            {
                PlayerPrefs.SetInt("TopScore", currentScore);
            }
        }

        public void UpdateEndGameTime()
        {
            float topTime = PlayerPrefs.GetFloat("TopTime");
            PlayerPrefs.SetFloat("EndGameTime", timeManager.timeLeft);

            if (timeManager.timeLeft >= topTime)
            {
                PlayerPrefs.SetFloat("TopTime", timeManager.timeLeft);
            }
            timeManager.GameOver();
        }

        public void ToggleMouseCursor(bool onOff)
        {
            if (!onOff)
            { Cursor.lockState = CursorLockMode.Locked; }
            else
            { Cursor.lockState = CursorLockMode.None; }

            Cursor.visible = onOff;
        }
    }
}
