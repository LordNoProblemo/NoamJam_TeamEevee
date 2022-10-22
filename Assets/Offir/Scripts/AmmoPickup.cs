using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IACFPSController.Gameplay
{
    [AddComponentMenu("IAC Axe Game/Objects/Ammo Pickup")]
    public class AmmoPickup : MonoBehaviour
    {
        [Tooltip("How much ammo to add")]
        public int ammoValue = 1;

        [Tooltip("What tag does the player have?")]
        public string playerTag = "Player";

        private AxeThrowAdvanced playerController;
        private bool isPickedUp = false;

        private void Awake()
        {
            //playerController = FindObjectOfType<AxeThrowWithAnimation>();
            
            isPickedUp = true;
            Invoke("EnablePickup",1f);
        }


        private void OnTriggerEnter(Collider other)
        {
            if(isPickedUp)
            {
                return;
            }

            if(other.CompareTag(playerTag))
            {
                playerController = other.GetComponentInParent<AxeThrowAdvanced>();
                isPickedUp = true;
                AddAmmoToPlayer();
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if(isPickedUp)
            {
                return;
            }

            if (collision.collider.CompareTag(playerTag))
            {
                playerController = collision.collider.GetComponentInParent<AxeThrowAdvanced>();
                isPickedUp = true;
                AddAmmoToPlayer();
            }
        }

        public void AddAmmoToPlayer()
        {
            if (playerController)
            {
                playerController.AddAxeAmmo(ammoValue);
                Destroy(this.gameObject);
            }
        }

        public void EnablePickup()
        {
            isPickedUp = false;
        }


    }
}
