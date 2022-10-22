using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    [SerializeField] protected Animator idleAnimation;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] protected GameObject weaponPrefab;
    [SerializeField] protected float destroyOnDelay = 0.0f;
    protected GameObject weapon;
    [SerializeField] float projectileDelay = 0.3f, meleeCooldown = 0.1f, damageCooldown=0.3f;
    bool isAttacking, isDamaged;
    [SerializeField] bool reloadAfterDeath = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHP = maxHP;
        if (weapon != null)
            weapon.SetActive(false);
        if (idleAnimation != null)
            idleAnimation.Play(gameObject.tag + "_Idle_Anim");
        isAttacking = false;
        isDamaged = false;
    }

    public virtual void OnDeath()
    {
        if (idleAnimation != null)
            idleAnimation.Play(gameObject.tag + "_Death_Anim");
        StartCoroutine(DestroyOnDeath());
    }

    IEnumerator DestroyOnDeath()
    {
        yield return new WaitForSeconds(destroyOnDelay);
        GameObject.Destroy(gameObject);
        if (reloadAfterDeath)
            SceneManager.LoadScene("SampleScene");
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
    }

    public void Damage(int amount)
    {
        currentHP = Mathf.Max(0, currentHP - amount);
        if (idleAnimation != null)
            StartCoroutine(TakingDamage());
    }

    IEnumerator TakingDamage()
    {
        isDamaged = true;
        try
        {
            idleAnimation.Play(gameObject.tag + "_Hit_Anim");
        }
        catch { }
        yield return new WaitForSeconds(damageCooldown);
        isDamaged = false;
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

    protected virtual void Jump()
    {
        if (onGround && !isJumping) {
            isJumping = true;
            lastJumpTime = Time.time;
            rb.AddForce(new Vector2(0, jumpPower), ForceMode2D.Impulse);
            if (idleAnimation != null && !isAttacking && !isDamaged)
                idleAnimation.Play(gameObject.tag + "_Jump_Anim");
            // do animation
        }
    }

    protected void MoveRight() 
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);
        isLookingRight = true;
        if (idleAnimation != null && !isAttacking && onGround && !isDamaged)
            idleAnimation.Play(gameObject.tag + "_Walking_Anim");
        // do animation
    }

    protected void MoveLeft()
    {
        rb.velocity = new Vector2(-speed, rb.velocity.y);
        isLookingRight = false;
        if (idleAnimation != null && !isAttacking && onGround && !isDamaged)
            idleAnimation.Play(gameObject.tag + "_Walking_Anim");
        // do animation
    }

    protected void StopMovement()
    {
        rb.velocity = new Vector2(0, rb.velocity.y);
    }

    protected virtual void Bump()
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
        if (Time.time - lastAttack < weaponPrefab.GetComponent<MeleeWeapon>().delay + meleeCooldown)
            return;
        weapon = GameObject.Instantiate(weaponPrefab, WeaponSpawn(), Quaternion.identity);
        weapon.GetComponent<MeleeWeapon>().SetOwner(gameObject);
        weapon.GetComponent<MeleeWeapon>().Activate();
        isAttacking = true;

        if (idleAnimation != null)
            idleAnimation.Play(gameObject.tag + "_Attack_Anim");

        // do animation
        lastAttack = Time.time;
        StartCoroutine(destroyWeapon());
    }

    IEnumerator destroyWeapon()
    {
        yield return new WaitForSeconds(weaponPrefab.GetComponent<MeleeWeapon>().delay);
        weapon = null;
        isAttacking = false;

    }

    Vector2 WeaponSpawn()
    {
        Vector2 weaponSpawn = gameObject.transform.position;
        if (isLookingRight)
            weaponSpawn += 1.1f * Vector2.right;
        else
            weaponSpawn += 1.1f * Vector2.left;
        return weaponSpawn;
    }
    void HandleWeaponLocation()
    {
        if (weapon == null)
            return;
        

        weapon.transform.position = WeaponSpawn();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        Move();
        HandleAnimationDirection();
        HandleWeaponLocation();
    }
}
