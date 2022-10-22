using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IACFPSController.Managers;

namespace IACFPSController.Gameplay
{
    [AddComponentMenu ("IAC Axe Game/Objects/Destination Trigger")]
    public class DestinationTrigger : MonoBehaviour
    {
        public string playerTag = "Player";
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(playerTag))
            {
                FindObjectOfType<EndGameManager>().DestinationReached();
            }

        }
    }
}
