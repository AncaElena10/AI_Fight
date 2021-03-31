using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_Enemy : MonoBehaviour
{
    private Image healthBar;
    public float currentHealth;
    private float maxHealth = 100f;

    //public Enemy _enemy = new Enemy();
    public HealthManager _hm = new HealthManager();

    // Start is called before the first frame update
    void Start()
    {
        healthBar = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        currentHealth = _hm.GetEnemyHealth();
        healthBar.fillAmount = currentHealth / maxHealth;
    }
}
