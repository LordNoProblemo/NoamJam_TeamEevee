using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreature : MonoBehaviour
{
    public Rigidbody2D rb;
    public int maxHP;
    protected int currentHP;
    public int jumpPower, speed;
    private bool onGround = false;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }

    public virtual void OnDeath()
    {
        GameObject.Destroy(gameObject);
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    public void Damage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
    }

    public int getCurrentHP()
    {
        return currentHP;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.transform.localPosition.y < gameObject.transform.localPosition.y)
            onGround = true;
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.transform.localPosition.y < gameObject.transform.localPosition.y)
            onGround = false;
    }

    protected void Jump()
    {
        if (onGround)
            rb.AddForce(new Vector2(0, jumpPower));
    }

    protected void MoveRight() 
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }

    protected void MoveLeft()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
    }

    protected void StopMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }
    protected abstract void Move();
    
    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
    }
}
