using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCreature : MonoBehaviour
{
    public Rigidbody2D rb;
    public int maxHP;
    protected int currentHP;
    public int jumpPower, speed;
    protected bool onGround = false;
    private GameObject lastGround;
    public bool isLookingRight;
    protected bool isJumping;
    float lastJumpTime = -Mathf.Infinity;
    float lastPushed = -Mathf.Infinity;
    float lastProjectileShot = -Mathf.Infinity;
    float lastAttack = -Mathf.Infinity;
    [SerializeField] private Animator idleAnimation;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] protected GameObject weapon;
    [SerializeField] float projectileDelay = 0.3f, meleeCooldown = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        currentHP = maxHP;
        if (weapon != null)
            weapon.SetActive(false);
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
        Debug.Log("[" + gameObject.name + "] " + currentHP + "/" + maxHP);
    }

    public int getCurrentHP()
    {
        return currentHP;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (ContactPoint2D pnt in collision.contacts)
            if (pnt.normal.y > 0)
            {
                onGround = true;
                isJumping = false;
                lastGround = collision.gameObject;
                return;
            }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject == lastGround)
            onGround = true;
        if (isJumping && rb.velocity.y == 0 && lastJumpTime + 1 < Time.time)
            isJumping = false;
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == lastGround)
        {
            onGround = false;
            lastGround = null;
        }    
    }

    protected void Jump()
    {
        if (onGround && !isJumping) {
            isJumping = true;
            lastJumpTime = Time.time;
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            // do animation
        }
    }

    protected void MoveRight() 
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        isLookingRight = true;
        // do animation
    }

    protected void MoveLeft()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
        isLookingRight = false;
        // do animation
    }

    protected void StopMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
        // do animation
    }

    protected void Bump()
    {
        if (isLookingRight)
            MoveLeft();
        else
            MoveRight();
        Jump();
    }
    
    public void Push(Vector2 velocity)
    {
        rb.velocity = velocity;
        lastPushed = Time.time;
    }
    protected abstract void Move();

    protected void HandleAnimationDirection()
    {
        if (idleAnimation == null)
            return;
        foreach (SpriteRenderer sp in idleAnimation.GetComponentsInChildren<SpriteRenderer>(true))
        {
            sp.flipX = isLookingRight;
        }
    }

    protected void ShootProjectile()
    {
        if (Time.time - lastProjectileShot < projectileDelay)
            return;
        // do animation
        Vector2 projectileSpawn = gameObject.transform.position;
        if (isLookingRight)
            projectileSpawn += Vector2.right;
        else
            projectileSpawn += Vector2.left;
        GameObject projectileObject = GameObject.Instantiate(projectilePrefab, projectileSpawn, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.SetOwner(gameObject);
        projectile.SetDirection(isLookingRight);
        lastProjectileShot = Time.time;
    }

    protected void Attack()
    {
        if (Time.time - lastAttack < weapon.GetComponent<MeleeWeapon>().delay + meleeCooldown)
            return;
        weapon.GetComponent<MeleeWeapon>().Activate();
        // do animation
        lastAttack = Time.time;
    }
    void HandleWeaponLocation()
    {
        if (weapon == null)
            return;
        Vector2 weaponSpawn = gameObject.transform.position;
        if (isLookingRight)
            weaponSpawn += Vector2.right;
        else
            weaponSpawn += Vector2.left;

        weapon.transform.position = weaponSpawn;
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        Move();
        HandleAnimationDirection();
        HandleWeaponLocation();
    }
}
