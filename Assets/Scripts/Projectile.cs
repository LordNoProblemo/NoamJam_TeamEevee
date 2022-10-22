using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : DamageObject
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] float speed;
    [SerializeField] SpriteRenderer sprite;
    Vector2 velocity;
    private void Start()
    {
        destroyOnHit = true;
    }

    public void SetDirection(bool is_right)
    {
        if (is_right)
            rb.velocity = speed * Vector2.right;
        else
            rb.velocity = speed * Vector2.left;
        sprite.flipX = is_right;

    }
}
