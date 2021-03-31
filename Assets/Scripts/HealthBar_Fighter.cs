using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Fighter : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth = 100f;

    //public Fighter _fighter = new Fighter();
    public HealthManager _hm = new HealthManager();

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = _hm.GetFighterHealth();
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
