using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject: MonoBehaviour
{
    public GameObject owner;
    public int damage;
    public bool destroyOnHit;
    float lastHitTime;
    public float damageDelay;

    private void Start()
    {
        lastHitTime = -Mathf.Infinity;
    }

    private void DoDamage(Collision2D collision)
    {
        BaseCreature creature = collision.gameObject.GetComponent<BaseCreature>();

        if (creature != null && owner != creature.gameObject && Time.time - lastHitTime > damageDelay)
        {   
            creature.Damage(damage);
            if (creature.getCurrentHP() == 0)
                creature.OnDeath();
            lastHitTime = Time.time;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        DoDamage(collision);
        if (destroyOnHit)
            GameObject.Destroy(gameObject);

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        Debug.Log("MEOWY");
        DoDamage(collision);
    }

    
}
