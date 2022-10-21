using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public BaseCreature creature;
    public Text hpText;
    public Image healthBar;
    private float barSize;
    private void Start()
    {
        barSize = healthBar.rectTransform.anchoredPosition.x;
    }
    void setHealth()
    {
        hpText.text = creature.getCurrentHP() + "/" + creature.maxHP;
        
        healthBar.rectTransform.localScale = new Vector3(
            (float)creature.getCurrentHP() / creature.maxHP,
            1
        );
        healthBar.rectTransform.anchoredPosition = new Vector3(
             barSize * (float)creature.getCurrentHP() / creature.maxHP,
            0
        );
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        setHealth();
    }
}
