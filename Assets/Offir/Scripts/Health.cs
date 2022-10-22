using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    public float maxHealth = 3;
    public float deathHealthThreshold = 0;
    public UnityEvent TriggeredEvent;
    public GameObject HealthIconsSection;
    public GameObject HealthIconPrefab;

    private float currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        for (var i = 0; i < maxHealth; i++)
        {
            Instantiate(HealthIconPrefab, HealthIconsSection.transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }
    
    public void OnDamageTaken(float damage)
    {
        currentHealth -= damage;
        var numIconsToRemove = currentHealth <= 0 ? HealthIconsSection.transform.childCount : (int)Math.Ceiling(damage);
        for (var i = 0; i < numIconsToRemove; i++)
        {
            Destroy(HealthIconsSection.transform.GetChild(HealthIconsSection.transform.childCount - 1).gameObject);
        }

        if (currentHealth > deathHealthThreshold) return;

        currentHealth = deathHealthThreshold;
        TriggeredEvent.Invoke();
    }

    public void OnHealthAdded(float health)
    {
        currentHealth += health;
        if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }

        var numIconsToAdd = ((int) currentHealth)- HealthIconsSection.transform.childCount;
        for (var i = 0; i < numIconsToAdd; i++)
        {
            Instantiate(HealthIconPrefab, HealthIconsSection.transform);
        }
    }
}
