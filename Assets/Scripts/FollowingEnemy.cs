using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : BaseCreature
{
    bool contacted_player = false;

    protected override void Move()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null || isJumping)
            return;
        if (contacted_player)
        {
            Bump();
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
    }

    protected override void OnCollisionExit2D(Collision2D collision)
    {
        base.OnCollisionExit2D(collision);
        if (collision.gameObject.tag == "Player")
            contacted_player = false;
    }
}
