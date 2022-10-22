using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IACFPSController.Gameplay
{

    [AddComponentMenu ("IAC Axe Game/Player/Axe Throw (Simple)")]
    public class AxeThrowWithAnimation : MonoBehaviour
    {
        [Tooltip("Place the Axe Prefab here that you want the character to throw")]
        public GameObject axeObject;
        [Tooltip("Place the Axe Origin Empty Game Object here. This is the location from which the axe will be thrown")]
        public Transform axeOrigin;
        [Tooltip("The base power of the axe being thrown")]
        public float throwPower = 20f;
        private float powerMultiplier = 0;

        [Tooltip("Connect the axe origin empty game object here, with an animator component")]
        public Animator axeHandAnim;

        // Start is called before the first frame update
        void Start()
        {
            powerMultiplier = 0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                if (axeHandAnim)
                {
                    axeHandAnim.SetTrigger("Throw");
                }
            }

            if (Input.GetButton("Fire1"))
            {
                powerMultiplier += Time.deltaTime;
                powerMultiplier = Mathf.Clamp(powerMultiplier, 0.25f, 1.5f);
            }

            if (Input.GetButtonUp("Fire1"))
            {
                CreateAxe();
                axeHandAnim.SetTrigger("Return");
                powerMultiplier = 0;
            }
        }

        public void CreateAxe()
        {
            GameObject axeTemp = Instantiate(axeObject, axeOrigin.position, axeOrigin.rotation);
            axeTemp.GetComponent<Rigidbody>().AddForce(axeTemp.transform.forward * throwPower * powerMultiplier, ForceMode.Impulse);
            axeTemp.GetComponent<Rigidbody>().AddTorque(axeTemp.transform.right * 50f * powerMultiplier);
        }
    }
}
