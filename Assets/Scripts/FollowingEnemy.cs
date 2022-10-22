using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : BaseCreature
{
    bool contacted_player = false;
    bool contacted_damage = false;
    protected override void Move()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || isJumping)
            return;
        if (contacted_player || contacted_damage)
        {
            Bump();
            contacted_damage = false;
            return;
        }
        else if (player.transform.localPosition.x > transform.localPosition.x)
            MoveRight();
        else if (player.transform.localPosition.x < transform.localPosition.x)
            MoveLeft();
        else
            StopMovement();
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
        
        if (collision.gameObject.tag == "Player")
            contacted_player = true;
        if (collision.gameObject.GetComponent<DamageObject>() != null)
            contacted_damage = true;
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        if (collision.gameObject.tag == "Player")
            contacted_player = false;
        if (collision.gameObject.GetComponent<DamageObject>() != null)
            contacted_damage = false;
    }
}
