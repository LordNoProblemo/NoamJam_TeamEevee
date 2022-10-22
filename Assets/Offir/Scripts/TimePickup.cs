using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IACFPSController.Managers;

namespace IACFPSController.Gameplay
{
    [AddComponentMenu ("IAC Axe Game/Objects/Time Pickup")]
    public class TimePickup : MonoBehaviour
    {
        private TimeManager timeManager;

        public string playerTag = "Player";
        public float timeToAdd = 5f;


        private void Awake()
        {
            timeManager = FindObjectOfType<TimeManager>();
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                if (timeManager)
                {
                    timeManager.AddTime(timeToAdd);
                }
                Destroy(this.gameObject);
            }
        }
    }
}
