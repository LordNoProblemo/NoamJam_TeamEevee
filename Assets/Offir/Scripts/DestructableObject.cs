using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace IACFPSController.Gameplay
{ 
    [AddComponentMenu("IAC Axe Game/Objects/Destructable Object")]
    public class DestructableObject : MonoBehaviour
    {
        [Tooltip("The tag for the weapon")]
        public string weaponTag = "Weapon";

        [Tooltip("Place the Destructable Object you want to place")]
        public GameObject destructableObject;
        private Rigidbody[] destructableParts;
        private bool isHit = false;


        [Header("Hit Force Settings")]
        [Tooltip("Strength of the explosive force")]
        public float forceMultiplier = 1f;
        [Tooltip("Radius of the explosive force")]
        public float forceRange = 1f;

        [Header("Destroy Object Settings")]
        [Tooltip("Should the game object be destroyed after being Hit")]
        public bool destroyObject = false;
        [Tooltip("How long to wait before destroying game object")]
        public float destroyTimer = 5f;

        private void Start()
        {
            isHit = false;
        }

        private void OnCollisionEnter(Collision other)
        {
            if ((other.collider.CompareTag(weaponTag)) && !isHit)
            {
                isHit = true;
                this.gameObject.SetActive(false);
                Destroy(this.gameObject, 5f);

                CreateDesturctable();
                ExplodeDestructable(other);

            }
        }

        public void CreateDesturctable()
        {
            GameObject tempDestructableObject = Instantiate(destructableObject, transform.position, transform.rotation);
            destructableParts = tempDestructableObject.GetComponentsInChildren<Rigidbody>();

            if (destroyObject)
            {
                Destroy(tempDestructableObject, destroyTimer);
            }
        }

        public void ExplodeDestructable(Collision other)
        {
            foreach (var rb in destructableParts)
            {
                if (rb)
                {
                    rb.AddExplosionForce(other.relativeVelocity.magnitude * Random.Range(0.1f, 0.5f) * forceMultiplier,
                                         other.contacts[0].point, forceRange * Random.Range(0.5f, 2f),
                                         0.1f,
                                         ForceMode.Impulse);
                }
            }
        }

    }
}
