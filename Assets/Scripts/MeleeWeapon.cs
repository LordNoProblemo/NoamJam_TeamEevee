using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : DamageObject
{
    float activatedTime;
    [SerializeField] public float delay=0.4f;
    public void Activate()
    {
        gameObject.SetActive(true);

        StartCoroutine(Destroy());
    }
    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(delay);
        GameObject.Destroy(gameObject);
    }
}
