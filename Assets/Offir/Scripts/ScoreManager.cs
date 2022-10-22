using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IACFPSController.Managers
{
    [AddComponentMenu("IAC Axe Game/Managers/Score Manager")]
    public class ScoreManager : MonoBehaviour
    {

        [Header("Score Information")]
        [Tooltip("Score needed to win game")]
        public int winScore = 15;
        private int currentScore = 0;
        private int topScore = 0;
        

        [Header("UI Elements")]
        public Text scoreText;
        public Text topScoreText;
        public Animator scoreAnimation;

        private bool gameOver;
        [HideInInspector] public bool checkForWinScore;

        private void Start()
        {
            gameOver = false;
            topScore = PlayerPrefs.GetInt("TopScore",0);

            if (scoreText)
            {
                UpdateScore();
            }

            if (topScoreText)
            {
                topScoreText.text = topScore.ToString();
            }
            else { Debug.Log("Top Score Is: " + topScore); }
        }

        public void Update()
        {
            if(checkForWinScore && currentScore >= winScore)
            {
                EndGame();
            }
        }

        public void AddToScore(int scoreToAdd)
        {
            if(gameOver)
            { return; }

            currentScore += scoreToAdd;
            if(currentScore < 0)
            {
                currentScore = 0;
            }

            if(scoreAnimation)
            {
                scoreAnimation.gameObject.SetActive(true);
                if (scoreToAdd > 0)
                {
                    scoreAnimation.GetComponent<Text>().text = "+" + scoreToAdd;
                }
                else
                {
                    scoreAnimation.GetComponent<Text>().text = "" + scoreToAdd;
                }
                Invoke("AnimationOff", scoreAnimation.GetCurrentAnimatorStateInfo(0).length);
            }


            UpdateScore();
        }

        public void AnimationOff()
        {
            scoreAnimation.gameObject.SetActive(false);
        }

        public void UpdateScore()
        {
            if (scoreText)
            {
                scoreText.text = currentScore.ToString();
            }
            else
            {
                Debug.Log("Current Score is : " + currentScore);
            }
        }

        public int GetCurrentScore()
        {
            return currentScore;
        }

        public void EndGame()
        {
            gameOver = true;
            EndGameManager endManager = FindObjectOfType<EndGameManager>();
            if(endManager)
            {
                endManager.ScoreReached();
            }
        }
    }
}