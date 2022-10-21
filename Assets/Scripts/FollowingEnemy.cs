using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingEnemy : BaseCreature
{
    protected override void Move()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player.transform.localPosition.x > transform.localPosition.x)
            MoveRight();
        else if (player.transform.localPosition.x < transform.localPosition.x)
            MoveLeft();
        else
            StopMovement();
    }
}
