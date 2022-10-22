using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IACFPSController.Managers;


namespace IACFPSController.Gameplay
{
    [AddComponentMenu("IAC Axe Game/Objects/Score Pickup")]
    public class ScorePickup : MonoBehaviour
    {
        private ScoreManager scoreManager;

        public string playerTag = "Player";
        public int scoreValue = 1;


        private void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                if (scoreManager)
                {
                    scoreManager.AddToScore(scoreValue);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
