using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageObject: MonoBehaviour
{
    public GameObject owner;
    public int damage;
    public bool destroyOnHit;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BaseCreature creature = collision.gameObject.GetComponent<BaseCreature>();
        if (creature != null && owner != creature.gameObject)
        {
            creature.Damage(damage);
            if (creature.getCurrentHP() == 0)
                creature.OnDeath();
        }

        if (destroyOnHit)
            GameObject.Destroy(gameObject);

    }
}
