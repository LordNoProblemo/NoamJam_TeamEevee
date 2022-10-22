using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : DamageObject
{
    float activatedTime;
    [SerializeField] public float delay=0.4f;
    public void Activate()
    {
        activatedTime = Time.time;
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Time.time - activatedTime > delay)
        {
            gameObject.SetActive(false);
        }
    }
}
