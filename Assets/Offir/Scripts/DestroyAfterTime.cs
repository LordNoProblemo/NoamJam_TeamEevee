using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IACFPSController.Gameplay
{
    [AddComponentMenu("IAC Axe Game/Objects/Destroy Object")]
    public class DestroyAfterTime : MonoBehaviour
    {
        [Tooltip("Destroy this object after x Seconds")]
        public float destroyTime = 5f;

        public void Start()
        {
            Destroy(this.gameObject, destroyTime);
        }
    }
}