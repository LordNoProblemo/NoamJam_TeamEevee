using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreature : MonoBehaviour
{
    public Rigidbody2D rb;
    public int maxHP;
    protected int currentHP;
    private bool onGround = false;
    public int jumpPower, speed;
    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
    }


    protected void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Meow");
        if (collision.gameObject.tag == "Ground")
        {
            Debug.Log("Meowy");
            if (collision.gameObject.transform.localPosition.y < gameObject.transform.localPosition.y)
                onGround = true;
        }
    }

    protected void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
            if (collision.gameObject.transform.localPosition.y < gameObject.transform.localPosition.y)
                onGround = false;
    }

    protected void Jump()
    {
        if (onGround)
            rb.AddForce(new Vector2(0, 10 * jumpPower));
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
