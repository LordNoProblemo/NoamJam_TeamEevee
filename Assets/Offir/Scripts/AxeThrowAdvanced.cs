using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IACFPSController.Gameplay
{
    [AddComponentMenu("IAC Axe Game/Player/Axe Throw (Advanced)")]
    public class AxeThrowAdvanced : MonoBehaviour
    {
        [Header("Axe Creation Settings")]
        [Tooltip("Place the Axe Prefab here that you want the character to throw")]
        public GameObject axeObject;
        [Tooltip("Place the Axe Origin Empty Game Object here. This is the location from which the axe will be thrown")]
        public Transform axeOrigin;

        [Header("Behavior Settings")]
        [Tooltip("The base power of the axe being thrown")]
        public float throwPower = 20f;
        [Tooltip("Specify the delay in seconds between throws")]
        public float throwDelay = 0.5f;
        [Tooltip("Should the Axe auto aim towards center of screen?")]
        public bool autoCenter = true;
        [Tooltip("Apporximate the distance from player to which the axe will reach")]
        public float centerDistance = 20f;

        [Header("Ammo Settings")]
        [Tooltip("Start Ammo Count")]
        public int startAmmo = 5;
        [Tooltip("Max Ammo Count - 0 will create 9999 as max ammo")]
        public int maxAmmo = 10;


        [Header("UI Elements")]
        [Tooltip("Reference to UI element for Ammo Count")]
        public Text axeAmmoText;
        public Image axeCooldown;
        public Image axeStrength;
        public bool hideCooldown = false;

        [Header("Advanced Settings")]
        [Tooltip("The gfx object of the axe in hand")]
        public GameObject axeGfx;
        [Tooltip("Connect the axe origin empty game object here, with an animator component")]
        public Animator axeAnimator;


        //Private Variables
        private float powerMultiplier = 0;
        private int currentAmmo;
        private bool allowThrow;
        private float throwTimer;
        private bool startedThrow;
        private Vector3 originalAxePos;


        // Start is called before the first frame update
        void Start()
        {
            powerMultiplier = 0f;
            throwTimer = 0;
            startedThrow = false;

            if (startAmmo == 0)
            {
                ZeroAmmo();
            }
            if (maxAmmo == 0)
            {
                maxAmmo = 99999;
            }

            originalAxePos = axeOrigin.localEulerAngles;

            currentAmmo = startAmmo;
            UpdateAmmoUI();
            EnableThrowStrengthUI(false);
            UpdateThrowStrength();
            UpdateAxeCooldown();

        }

        // Update is called once per frame
        void Update()
        {
            if (currentAmmo <= 0)
            {
                return;
            }

            if (!allowThrow)
            {
                throwTimer += Time.deltaTime;
                UpdateAxeCooldown();

                if (throwTimer >= throwDelay)
                {
                    throwTimer = 0;
                    axeOrigin.localEulerAngles = originalAxePos;
                    EnableAxeThrow(true);
                }

                return;
            }

            ThrowInput();
        }

        private void ThrowInput()
        {
            if (Input.GetButtonDown("Fire1"))
            {
                startedThrow = true;

                if (axeAnimator)
                {
                    axeAnimator.SetTrigger("Throw");
                }

                EnableThrowStrengthUI(true);

            }

            if (startedThrow)
            {
                if (Input.GetButton("Fire1"))
                {

                    powerMultiplier += Time.deltaTime;
                    powerMultiplier = Mathf.Clamp(powerMultiplier, 0.25f, 1.5f);
                    UpdateThrowStrength();
                }

                if (Input.GetButtonUp("Fire1"))
                {
                    CreateAxe();

                    if (axeAnimator)
                    {
                        axeAnimator.SetTrigger("Return");
                    }
                    startedThrow = false;
                    powerMultiplier = 0;


                    EnableThrowStrengthUI(false);
                    EnableAxeThrow(false);
                }
            }
        }

        public void CreateAxe()
        {
            if(autoCenter)
            {
                AimTowardCenter();
            }
            GameObject axeTemp = Instantiate(axeObject, axeOrigin.position,  axeOrigin.rotation);

            axeTemp.GetComponent<Rigidbody>().AddForce(axeTemp.transform.forward * throwPower * powerMultiplier, ForceMode.Impulse);
            axeTemp.GetComponent<Rigidbody>().AddTorque(axeTemp.transform.right * 50f * powerMultiplier);

            currentAmmo--;

            if (currentAmmo <= 0)
            {
                currentAmmo = 0;
                ZeroAmmo();
            }

            UpdateAmmoUI();
        }

        public void EnableAxeThrow(bool enable)
        {
            if (axeGfx)
            {
                axeGfx.gameObject.SetActive(enable);
            }
            if(axeCooldown && hideCooldown)
            {
                axeCooldown.transform.parent.gameObject.SetActive(!enable);
            }
            
            allowThrow = enable;
        }

        public void ZeroAmmo()
        {
            if (axeGfx)
            {
                axeGfx.gameObject.SetActive(false);
            }
            if (axeCooldown)
            {
                axeCooldown.transform.parent.gameObject.SetActive(false);
            }
        }

        public void AimTowardCenter()
        {
            Vector3 crosshairPoint = Vector3.zero;
            RaycastHit hitInfo;
            
            if(Physics.Raycast(Camera.main.transform.position,Camera.main.transform.forward, out hitInfo, centerDistance * powerMultiplier))
            {
                crosshairPoint = hitInfo.point;
            }
            else
            {
                crosshairPoint = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, centerDistance * powerMultiplier));
            }
           
            axeOrigin.LookAt(crosshairPoint);
        }
        


        public void AddAxeAmmo(int ammoCount)
        {
            if (currentAmmo == 0)
            {
                if (axeGfx)
                {
                    axeGfx.gameObject.SetActive(true);
                }
                if (axeCooldown)
                {
                    axeCooldown.transform.parent.gameObject.SetActive(true);
                }
            }
            currentAmmo += ammoCount;
            UpdateAmmoUI();
        }

        public void UpdateAmmoUI()
        {
            currentAmmo = Mathf.Clamp(currentAmmo, 0, maxAmmo);
            if (axeAmmoText)
            {
                axeAmmoText.text = currentAmmo.ToString();
            }
        }

        public void UpdateAxeCooldown()
        {
            if (axeCooldown)
            {
                axeCooldown.fillAmount = throwTimer / throwDelay;
            }
        }

        public void UpdateThrowStrength()
        {
            if (axeStrength)
            {
                axeStrength.fillAmount = powerMultiplier / 1.5f;
            }
        }

        public void EnableThrowStrengthUI(bool enable)
        {
            if (axeStrength)
            {
                axeStrength.transform.parent.gameObject.SetActive(enable);
            }
        }
    }
}
