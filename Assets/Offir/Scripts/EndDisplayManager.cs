using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IACFPSController.Managers
{
    [AddComponentMenu("IAC Axe Game/Managers/End Screen Data")]
    public class EndDisplayManager : MonoBehaviour
    {
        public bool timeToScore;
        public float secondsToPoint = 5f;

        [Header ("Score Text Fields")]
        public Text scoreText;
        public Text topScoreText;

        [Header ("Time Text Fields")]
        public Text timeText;
        public Text topTimeText;


        public void DisplayPointScore()
        {
            if(timeToScore)
            {
                int timePointScore = Mathf.FloorToInt(PlayerPrefs.GetFloat("EndGameTime") / secondsToPoint) + PlayerPrefs.GetInt("EndGameScore");
                PlayerPrefs.SetInt("EndGameScore",  timePointScore);

                if(timePointScore >= PlayerPrefs.GetInt("TopScore"))
                {
                    PlayerPrefs.SetInt("TopScore", timePointScore);
                }
            }

            if (scoreText)
            {
                scoreText.text = PlayerPrefs.GetInt("EndGameScore").ToString();
            }
            if (topScoreText)
            {
                topScoreText.text = PlayerPrefs.GetInt("TopScore").ToString();
            }

            
        }


        public void DisplayTimeScore()
        {
            if (timeText)
            {
                timeText.text = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("EndGameTime")).ToString("mm':'ss");
            }

            if (topTimeText)
            {
                topTimeText.text = TimeSpan.FromSeconds(PlayerPrefs.GetFloat("TopTime")).ToString("mm':'ss");
            }

        }
    }

}
