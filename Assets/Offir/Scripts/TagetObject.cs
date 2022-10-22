using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IACFPSController.Managers;

namespace IACFPSController.Gameplay
{
    [AddComponentMenu("IAC Axe Game/Objects/Target Object")]
    public class TagetObject : MonoBehaviour
    {
        [Tooltip("The Tag for the weapon")]
        public string weaponTag = "Weapon";

        private ScoreManager scoreManager;
        private TimeManager timeManager;

        private bool isHit = false;
        public int scoreValue;


        [Header("Special Behaviour")]
        [Tooltip("Should this object react physically? (Requires target to be set up as Kinematic)")]
        public bool enablePhysics;
        [Tooltip("Should this target add time to the timer")]
        public bool addToTimer;
        [Tooltip("Time to Add")]
        public float timeValue = 10f;

        [Header("Target Health")]
        [Tooltip("Number of hits to destroy target")]
        public int hitPoints = 1;
        [Tooltip("Delay time between each hits")]
        public float hitDelay = 1f;


        [Header("Destroy Settings")]
        [Tooltip("Should the object be destroyed after being hit?")]
        public bool destroyObject;
        [Tooltip("How much time to wait until destroying the target in seconds, after hit")]
        public float destroyTimer = 5f;


        private void Awake()
        {
            scoreManager = FindObjectOfType<ScoreManager>();
            timeManager = FindObjectOfType<TimeManager>();
        }

        void Start()
        {
            isHit = false;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.collider.CompareTag(weaponTag) && !isHit)
            {
                TargetHitEvent();
            }
        }

        private void TargetHitEvent()
        {
            hitPoints--;
            isHit = true;
            if (hitPoints <= 0)
            {
                TargetDestroyedEvent();
            }
            else
            {
                Invoke("ResetIsHit", 1f);
            }

        }

        private void TargetDestroyedEvent()
        {
            if (scoreManager)
            {
                scoreManager.AddToScore(scoreValue);
            }
            if (addToTimer && timeManager)
            {
                timeManager.AddTime(timeValue);
            }
            if (enablePhysics)
            {
                GetComponent<Rigidbody>().isKinematic = false;
            }
            if (destroyObject)
            {
                Destroy(this.gameObject, destroyTimer);
            }
        }

        private void ResetIsHit()
        {
            isHit = false;
        }
    }
}

