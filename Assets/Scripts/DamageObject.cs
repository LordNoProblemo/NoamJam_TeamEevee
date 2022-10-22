using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject: MonoBehaviour
{
    [SerializeField] protected GameObject owner;
    [SerializeField] int damage;
    [SerializeField] float damageDelay;
    [SerializeField] protected bool destroyOnHit;
    float lastHitTime;

    private void Start()
    {
        lastHitTime = -Mathf.Infinity;
    }

    private void DoDamage(Collision2D collision)
    {
        BaseCreature creature = collision.gameObject.GetComponent<BaseCreature>();
        if (creature != null && owner != creature.gameObject && Time.time - lastHitTime > damageDelay)
        {   
            if (collision.gameObject != GameObject.FindGameObjectWithTag("Player") || !collision.gameObject.GetComponent<Player>().Shielded())
                creature.Damage(damage);
            if (creature.getCurrentHP() == 0)
                creature.OnDeath();
            lastHitTime = Time.time;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        DoDamage(collision);
        if (destroyOnHit && collision.gameObject != owner)
            GameObject.Destroy(gameObject);

    }

    public void SetOwner(GameObject newOwner)
    {
        owner = newOwner;
    }
    
}
