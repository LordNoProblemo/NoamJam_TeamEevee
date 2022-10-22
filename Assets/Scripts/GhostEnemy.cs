using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostEnemy : BaseCreature
{
    bool contacted_player = false;
    bool contacted_damage = false;
    [SerializeField] float distanceFromPlayer=0.1f;
    [SerializeField] float falseJumpDelay = 0.3f;
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
        else if (player.transform.localPosition.x > transform.localPosition.x + distanceFromPlayer)
            MoveRight();
        else if (player.transform.localPosition.x < transform.localPosition.x - distanceFromPlayer)
            MoveLeft();
        else
            StopMovement();

        if (player.transform.localPosition.y > transform.localPosition.y + distanceFromPlayer)
            rb.velocity = new Vector2(rb.velocity.x / speed, 1).normalized * speed;
        else if (player.transform.localPosition.y < transform.localPosition.y - distanceFromPlayer)
            rb.velocity = new Vector2(rb.velocity.x / speed, -1).normalized * speed;
    }

    protected override void Bump()
    {
        base.Bump();
        isJumping = true;
        StartCoroutine(cancelFalseJump());
    }

    IEnumerator cancelFalseJump()
    {
        yield return new WaitForSeconds(falseJumpDelay);
        isJumping = false;
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

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        //if (player != null && Mathf.Abs(player.transform.localPosition.x - transform.localPosition.x) < distanceFromPlayer && Mathf.Abs(player.transform.localPosition.y - transform.localPosition.y) < distanceFromPlayer)
          //  Attack();
    }
}
