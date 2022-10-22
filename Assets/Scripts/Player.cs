using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCreature
{
    [SerializeField] GameObject shield;
    [SerializeField] float shieldActiveTime=2.0f, shieldCooldown=5.0f;
    float nextShieldAvailableTime = -Mathf.Infinity;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

    }
    protected override void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            MoveRight();
        else if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        else
            StopMovement();

        if (Input.GetKey(KeyCode.Space) && !shield.activeSelf)
            Jump();
    }

    IEnumerator destroyShield()
    {
        yield return new WaitForSeconds(shieldActiveTime);
        shield.SetActive(false);

    }

    public bool Shielded()
    {
        return shield.activeSelf;
    }
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && weapon == null && !Shielded())
            Attack();
        if (Input.GetKeyDown(KeyCode.S) && !Shielded())
            ShootProjectile();
        if (Input.GetKeyDown(KeyCode.D) && Time.time > nextShieldAvailableTime)
        {
            nextShieldAvailableTime = Time.time + shieldActiveTime + shieldCooldown;
            shield.SetActive(true);
            // Switch to Idle animation
            StartCoroutine(destroyShield());
        }
    }
}
