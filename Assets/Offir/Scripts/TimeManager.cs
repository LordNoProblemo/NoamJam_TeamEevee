using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using IACFPSController.Managers;
using UnityEngine.SceneManagement;

namespace IACFPSController.Managers
{
    [AddComponentMenu("IAC Axe Game/Managers/Time Manager")]
    public class TimeManager : MonoBehaviour
    {
        private bool gameOver = false;

        [Tooltip("Define the starting time for the game")]
        public float startTime;
        [HideInInspector] public float timeLeft;
        private float topTime = 0;
        private TimeSpan timerDisplay;
        

        [Header("UI Elements")]
        public Text timerText;
        public Text topTimeText;
        public Color warningColor = Color.red;
        private Color originalColor;
        public Animator timeAnimation;

        private void Awake()
        {
        }

        // Start is called before the first frame update
        void Start()
        {
            topTime = PlayerPrefs.GetFloat("TopTime", 0);

            if(topTimeText)
            {
                TimeSpan topTimeDisplay = TimeSpan.FromSeconds(topTime);
                topTimeText.text = topTimeDisplay.ToString("mm':'ss");

            }

            gameOver = false;
            timeLeft = startTime;

            if (timerText)
            {
                originalColor = timerText.color;
            }

            InvokeRepeating("UpdateTimer", 0f, 1f);
        }

        // Update is called once per frame
        void Update()
        {
            if (gameOver)
            {
                if (Input.GetKey(KeyCode.Escape))
                    SceneManager.LoadScene("SampleScene");

                return;
            }

            timeLeft += Time.deltaTime;

            if (timeLeft <= 0)
            {
               EndGameEvent();
            }
        }

        public void UpdateTimer()
        {
            if (gameOver)
            { return; }

            timerDisplay = TimeSpan.FromSeconds(timeLeft);

            if (timerText)
            {
                if (timeLeft < 60f)
                {
                    timerText.color = warningColor;
                }
                else { timerText.color = originalColor; }
                timerText.text = timerDisplay.ToString("mm':'ss");
            }
            else
            {
                Debug.Log("Time Left: " + timerDisplay.ToString("mm' : 'ss"));
            }
        }

        public void AddTime(float timeToAdd)
        {

            if (gameOver)
            {
                return;
            }


            timeLeft += timeToAdd;


            if (timeAnimation)
            {
                AnimateAddTime(timeToAdd);
            }

            UpdateTimer();
        }

        public void AnimateAddTime(float addedTime)
        {
            timeAnimation.gameObject.SetActive(true);
            if (addedTime > 0)
            {
                timeAnimation.GetComponent<Text>().text = "+" + addedTime;
            }
            else
            {
                timeAnimation.GetComponent<Text>().text = "" + addedTime;
            }
            Invoke("AnimationOff", timeAnimation.GetCurrentAnimatorStateInfo(0).length);
        }

        private void AnimationOff()
        {
            timeAnimation.gameObject.SetActive(false);
        }

       public void GameOver()
        {
            gameOver = true;
        }

        public void EndGameEvent()
        {
            gameOver = true;
            EndGameManager endManager = FindObjectOfType<EndGameManager>();
            if(endManager)
            {
                endManager.TimeEnded();
            }
        }
    }
}
