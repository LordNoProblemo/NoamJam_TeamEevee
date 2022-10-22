using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCreature
{
    bool inAttack;
    // Start is called before the first frame update
    private void Start()
    {
        currentHP = PlayerPrefs.GetInt("currentHP", maxHP);
    }
    protected override void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
            MoveRight();
        else if (Input.GetKey(KeyCode.LeftArrow))
            MoveLeft();
        else
            StopMovement();

        if (Input.GetKey(KeyCode.Space))
            Jump();
    }
  
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) && !weapon.activeSelf)
            Attack();
        if (Input.GetKeyDown(KeyCode.S))
            ShootProjectile();
    }
}
