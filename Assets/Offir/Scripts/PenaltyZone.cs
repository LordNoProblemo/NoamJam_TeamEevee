using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IACFPSController.Managers;


namespace IACFPSController.Gameplay
{
    [AddComponentMenu("IAC Axe Game/Objects/Penalty Zone")]
    public class PenaltyZone : MonoBehaviour
    {
        private ScoreManager scoreManager;
        private TimeManager timeManager;
      

        public string playerTag = "Player";

        [Header ("Zone Settings")]
        public bool loseScore = true;
        public bool loseTime = true;
        public bool LoseHealth = true;
        public bool respawnPlayer = true;

        [Header ("Zone Values")]
        public int scoreValue = -1;
        public float timeValue = -5f;
        public float HealthPenalty = -1f;

        public Transform respawnLocation;
        private Transform playerObject;


        private void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            timeManager = FindObjectOfType<TimeManager>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(playerTag)) return;

            playerObject = other.gameObject.transform;
            var playerHealth = other.gameObject.GetComponent<Health>();

            if (scoreManager && loseScore)
            {
                scoreManager.AddToScore(scoreValue);
            }

            if (timeManager && loseTime)
            {
                timeManager.AddTime(timeValue);
            }

            if(respawnPlayer)
            {
                ResetPlayerLocation();
            }

            if (LoseHealth && playerHealth)
            {
                playerHealth.OnDamageTaken(HealthPenalty);
            }
        }

        private void ResetPlayerLocation()
        {

            playerObject.position = respawnLocation.position;

            // playerObject.rotation = respawnLocation.rotation;

        }
    }
}
