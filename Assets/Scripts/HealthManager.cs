using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    static float fighterHealth = 100f;
    static float enemyHealth = 100f;

    // Start is called before the first frame update
    void Start()
    {
        fighterHealth = 100f;
        enemyHealth = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        //print(" OOOOOOOOOOOOOO " + enemyHealth);
    }

    public void SetFighterHealth(float value)
    {
        fighterHealth -= value;
    }

    public void SetEnemyHealth(float value)
    {
        enemyHealth -= value;
        //print("ggg " + enemyHealth);
    }

    public float GetFighterHealth()
    {
        return fighterHealth;
    }

    public float GetEnemyHealth()
    {
        //print(enemyHealth);
        return enemyHealth;
    }
}
